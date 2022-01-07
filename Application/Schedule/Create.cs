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
        private readonly IUnitOfWork _unitOfWork;
        public ScheduleService(IUnitOfWork unitOfWork)
        {
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
                Class clas = new Class(rq.Class.Name);
                await _unitOfWork.Classes.CreateAsync(clas);
                scheduleDTO.ClassId = clas.Id;
            }

            if (!scheduleDTO.TrainerId.HasValue)
            {
                Trainer trainer = new Trainer(rq.Trainer.Name);
                await _unitOfWork.Trainers.CreateAsync(trainer);
                scheduleDTO.TrainerId = trainer.Id;
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