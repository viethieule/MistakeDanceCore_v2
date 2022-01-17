using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain;

namespace Application
{
    public class CreateScheduleRq : BaseRequest
    {
        public ScheduleDTO Schedule { get; set; }
        public BranchDTO Branch { get; set; }
        public ClassDTO Class { get; set; }
        public TrainerDTO Trainer { get; set; }
    }

    public class CreateScheduleRs : BaseResponse
    {
        public ScheduleDTO Schedule { get; set; }
    }

    public class CreateScheduleService : TransactionalAppService<CreateScheduleRq, CreateScheduleRs>
    {
        public CreateScheduleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override async Task<CreateScheduleRs> DoTransactionalRunAsync(CreateScheduleRq rq)
        {
            ScheduleDTO scheduleDTO = rq.Schedule;
            if (!scheduleDTO.ClassId.HasValue)
            {
                Class clas = new Class(rq.Class.Name);
                await _unitOfWork.Classes.CreateAsync(clas);
                await _unitOfWork.SaveChangesAsync();
                scheduleDTO.ClassId = clas.Id;
            }

            if (!scheduleDTO.TrainerId.HasValue)
            {
                Trainer trainer = new Trainer(rq.Trainer.Name);
                await _unitOfWork.Trainers.CreateAsync(trainer);
                await _unitOfWork.SaveChangesAsync();
                scheduleDTO.TrainerId = trainer.Id;
            }

            if (!scheduleDTO.BranchId.HasValue)
            {
                BranchDTO branchDTO = rq.Branch;
                Branch branch = new Branch(name: branchDTO.Name, abbreviation: branchDTO.Abbreviation, address: branchDTO.Address);
                await _unitOfWork.Branches.CreateAsync(branch);
                await _unitOfWork.SaveChangesAsync();
                scheduleDTO.BranchId = branch.Id;
            }

            Schedule schedule = new Schedule(
                song: scheduleDTO.Song,
                openingDate: scheduleDTO.OpeningDate,
                startTime: scheduleDTO.StartTime,
                daysPerWeek: scheduleDTO.DaysPerWeek,
                branchId: scheduleDTO.BranchId.Value,
                classId: scheduleDTO.ClassId.Value,
                trainerId: scheduleDTO.TrainerId.Value);

            await _unitOfWork.Schedules.CreateAsync(schedule);
            await _unitOfWork.SaveChangesAsync();

            if (rq.Schedule.TotalSessions.HasValue)
            {
                List<Session> sessions = schedule.GenerateSessions(rq.Schedule.TotalSessions.Value);
                foreach (Session session in sessions)
                {
                    await _unitOfWork.Sessions.CreateAsync(session);
                }

                await _unitOfWork.SaveChangesAsync();
            }

            return new CreateScheduleRs
            {
                Schedule = scheduleDTO
            };
        }
    }
}