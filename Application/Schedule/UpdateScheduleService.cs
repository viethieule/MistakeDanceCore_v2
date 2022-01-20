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
        private readonly ScheduleDTC _scheduleDTC;
        private readonly ClassDTC _classDTC;
        private readonly TrainerDTC _trainerDTC;
        private readonly SessionDTC _sessionDTC;
        private readonly ISessionsGenerator _sessionsGenerator;

        public UpdateScheduleService
        (
            ScheduleDTC scheduleDTC,
            ClassDTC classDTC,
            TrainerDTC trainerDTC,
            SessionDTC sessionDTC,
            ISessionsGenerator sessionsGenerator,
            IUnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            _scheduleDTC = scheduleDTC;
            _classDTC = classDTC;
            _trainerDTC = trainerDTC;
            _sessionDTC = sessionDTC;
            _sessionsGenerator = sessionsGenerator;
        }

        protected override async Task<UpdateScheduleRs> DoTransactionalRunAsync(UpdateScheduleRq rq)
        {
            ScheduleDTO scheduleDTO = rq.Schedule;
            if (!scheduleDTO.ClassId.HasValue)
            {
                ClassDTO classDTO = new ClassDTO() { Name = rq.Schedule.BranchName };
                await _classDTC.CreateAsync(classDTO);
                scheduleDTO.ClassId = classDTO.Id;
            }

            if (!scheduleDTO.TrainerId.HasValue)
            {
                TrainerDTO trainerDTO = new TrainerDTO() { Name = rq.Schedule.TrainerName };
                await _trainerDTC.CreateAsync(trainerDTO);
                scheduleDTO.TrainerId = trainerDTO.Id;
            }

            bool shouldUpdateSessions = schedule.ShouldUpdateSessions(newSchedule);

            await _scheduleDTC.UpdateAsync(scheduleDTO);

            if (shouldUpdateSessions)
            {
                List<SessionDTO> sessions = await _sessionDTC.ListByScheduleIdAsync(scheduleDTO.Id.Value);

                List<SessionDTO> newSessions = _sessionsGenerator.Generate(scheduleDTO);

                List<SessionDTO> toBeAddedSessions = newSessions.Where(x => sessions.Any(y => y.Date.Date == x.Date.Date)).ToList();
                List<SessionDTO> toBeRemovedSessions = sessions.Where(x => !newSessions.Any(y => y.Date.Date == x.Date.Date)).ToList();
                List<int> toBeRemovedSessionIds = toBeRemovedSessions.Select(y => y.Id.Value).ToList();

                sessions.RemoveAll(x => toBeRemovedSessionIds.Contains(x.Id.Value));
                sessions.AddRange(toBeAddedSessions);
                
                for (int i = 0; i < sessions.Count; i++)
                {
                    sessions[i] = i + 1;
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