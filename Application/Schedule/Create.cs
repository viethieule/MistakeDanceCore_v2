using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Shared;

namespace Application
{
    public class ScheduleDTO
    {
        public int Id { get; set; }
        public string Song { get; set; }
        public DateTime OpeningDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public List<DayOfWeek> DaysPerWeek { get; set; } = new List<DayOfWeek>();
        public int? BranchId { get; set; }
        public int? ClassId { get; set; }
        public int? TrainerId { get; set; }
        public int? TotalSessions { get; set; }
    }

    public class BranchDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Address { get; set; }
    }

    public class CreateScheduleRq
    {
        public ScheduleDTO Schedule { get; set; }
        public BranchDTO Branch { get; set; }
        public ClassDTO Class { get; set; }
        public TrainerDTO Trainer { get; set; }
    }

    public class TrainerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ClassDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateScheduleRs
    {
        public ScheduleDTO Schedule { get; set; }
    }

    public class ScheduleService
    {
        private readonly IRepository<Schedule> _scheduleRepo;
        private readonly IRepository<Trainer> _trainerRepo;
        private readonly IRepository<Class> _classRepo;
        private readonly IRepository<Session> _sessionRepo;
        private readonly IUnitOfWork _unitOfWork;
        public ScheduleService(
            IRepository<Schedule> scheduleRepo,
            IRepository<Trainer> trainerRepo,
            IRepository<Class> classRepo,
            IRepository<Session> sessionRepo,
            IUnitOfWork unitOfWork)
        {
            _scheduleRepo = scheduleRepo;
            _trainerRepo = trainerRepo;
            _classRepo = classRepo;
            _sessionRepo = sessionRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateScheduleRs> Run(CreateScheduleRq rq)
        {
            using (IDatabaseTransaction transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    CreateScheduleRs rs = await Create(rq);
                    transaction.Commit();

                    return rs;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<CreateScheduleRs> Create(CreateScheduleRq rq)
        {
            ScheduleDTO scheduleDTO = rq.Schedule;
            if (!scheduleDTO.ClassId.HasValue)
            {
                Class clas = await _unitOfWork.Classes.CreateAsync(new Class(rq.Class.Name));
                scheduleDTO.ClassId = clas.Id;
            }

            if (!scheduleDTO.TrainerId.HasValue)
            {
                Trainer trainer = await _unitOfWork.Trainers.CreateAsync(new Trainer(rq.Trainer.Name));
                scheduleDTO.TrainerId = trainer.Id;
            }

            Schedule schedule = await _unitOfWork.Schedules.CreateAsync(new Schedule(
                song: scheduleDTO.Song,
                openingDate: scheduleDTO.OpeningDate,
                startTime: scheduleDTO.StartTime,
                daysPerWeek: scheduleDTO.DaysPerWeek,
                branchId: scheduleDTO.BranchId.Value,
                classId: scheduleDTO.ClassId.Value,
                trainerId: scheduleDTO.TrainerId.Value));

            if (rq.Schedule.TotalSessions.HasValue)
            {
                List<Session> sessions = schedule.GenerateSessions(rq.Schedule.TotalSessions.Value);
                foreach (Session session in sessions)
                {
                    await _unitOfWork.Sessions.CreateAsync(session);
                }
            }

            return new CreateScheduleRs
            {
                Schedule = scheduleDTO
            };
        }
    }
}