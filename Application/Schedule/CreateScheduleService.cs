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
        private readonly ScheduleDTC _scheduleDTC;
        private readonly BranchDTC _branchDTC;
        private readonly TrainerDTC _trainerDTC;
        private readonly ClassDTC _classDTC;
        private readonly SessionDTC _sessionDTC;
        private readonly ISessionGenerator _sessionGenerator;

        public CreateScheduleService
        (
            ScheduleDTC scheduleDTC,
            BranchDTC branchDTC,
            TrainerDTC trainerDTC,
            ClassDTC classDTC,
            SessionDTC sessionDTC,
            ISessionGenerator sessionGenerator,
            IUnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            _scheduleDTC = scheduleDTC;
            _branchDTC = branchDTC;
            _trainerDTC = trainerDTC;
            _classDTC = classDTC;
            _sessionDTC = sessionDTC;
            _sessionGenerator = sessionGenerator;
        }

        protected override async Task<CreateScheduleRs> DoTransactionalRunAsync(CreateScheduleRq rq)
        {
            ScheduleDTO scheduleDTO = rq.Schedule;
            if (!scheduleDTO.ClassId.HasValue)
            {
                await _classDTC.CreateAsync(rq.Class);
                scheduleDTO.ClassId = rq.Class.Id;
            }

            if (!scheduleDTO.TrainerId.HasValue)
            {
                await _trainerDTC.CreateAsync(rq.Trainer);
                scheduleDTO.TrainerId = rq.Trainer.Id;
            }

            if (!scheduleDTO.BranchId.HasValue)
            {
                await _branchDTC.CreateAsync(rq.Branch);
                scheduleDTO.BranchId = rq.Branch.Id;
            }

            await _scheduleDTC.CreateAsync(scheduleDTO);

            if (rq.Schedule.TotalSessions.HasValue)
            {
                List<SessionDTO> sessions = _sessionGenerator.Generate(rq.Schedule);
                await _sessionDTC.CreateRangeAsync(sessions);
            }

            return new CreateScheduleRs
            {
                Schedule = scheduleDTO
            };
        }
    }
}