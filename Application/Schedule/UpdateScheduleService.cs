using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain;

namespace Application
{
    public class UpdateScheduleRq : BaseRequest
    {
        public ScheduleDTO Schedule { get; set; }
    }

    public class UpdateScheduleRs : BaseResponse
    {

    }

    public class UpdateScheduleService : TransactionalAppService<UpdateScheduleRq, UpdateScheduleRs>
    {
        public UpdateScheduleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override async Task<UpdateScheduleRs> DoTransactionalRunAsync(UpdateScheduleRq rq)
        {
            Schedule schedule = await _unitOfWork.Schedules.SingleByIdAsync(rq.Schedule.Id.Value);
            
            ScheduleDTO scheduleDTO = rq.Schedule;
            if (!scheduleDTO.ClassId.HasValue)
            {
                Class clas = new Class(scheduleDTO.ClassName);
                await _unitOfWork.Classes.CreateAsync(clas);
                await _unitOfWork.SaveChangesAsync();
                scheduleDTO.ClassId = clas.Id;
            }

            if (!scheduleDTO.TrainerId.HasValue)
            {
                Trainer trainer = new Trainer(scheduleDTO.TrainerName);
                await _unitOfWork.Trainers.CreateAsync(trainer);
                await _unitOfWork.SaveChangesAsync();
                scheduleDTO.TrainerId = trainer.Id;
            }

            Schedule newSchedule = new Schedule
            (
                song: scheduleDTO.Song,
                openingDate: scheduleDTO.OpeningDate,
                startTime: scheduleDTO.StartTime,
                daysPerWeek: scheduleDTO.DaysPerWeek,
                branchId: scheduleDTO.BranchId.Value,
                classId: scheduleDTO.ClassId.Value,
                trainerId: scheduleDTO.TrainerId.Value
            );

            bool shouldUpdateSessions = schedule.ShouldUpdateSessions(newSchedule);

            schedule.Update(newSchedule);

            if (shouldUpdateSessions)
            {
                List<Session> sessions = await _unitOfWork.Sessions.ListAsync(x => x.ScheduleId == schedule.Id);

                List<Session> newSessions = schedule.GenerateSessions(scheduleDTO.TotalSessions.Value);

                List<Session> toBeAddedSessions = newSessions.Where(x => sessions.Any(y => y.Date.Date == x.Date.Date)).ToList();
                List<Session> toBeRemovedSessions = sessions.Where(x => !newSessions.Any(y => y.Date.Date == x.Date.Date)).ToList();
                List<int> toBeRemovedSessionIds = toBeRemovedSessions.Select(y => y.Id).ToList();

                sessions.RemoveAll(x => toBeRemovedSessionIds.Contains(x.Id));
                sessions.AddRange(toBeAddedSessions);
                
                for (int i = 0; i < sessions.Count; i++)
                {
                    sessions[i].SetNumber(i + 1);
                }

                await _unitOfWork.Sessions.AddRangeAsync(toBeAddedSessions);
                _unitOfWork.Sessions.RemoveRange(toBeRemovedSessions);
                _unitOfWork.Sessions.UpdateRange(sessions);

                if (toBeRemovedSessions.Count > 0)
                {
                    List<Registration> toBeRemovedRegistrations = await _unitOfWork.Registrations.ListAsync(x => toBeRemovedSessionIds.Contains(x.Id));
                    _unitOfWork.Registrations.RemoveRange(toBeRemovedRegistrations);
                }
            }    

            await _unitOfWork.SaveChangesAsync();

            return new UpdateScheduleRs();
        }
    }
}