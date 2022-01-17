using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain;

namespace Application
{
    public class GetSessionsRq : BaseRequest
    {
        public DateTime Start { get; set; }
    }

    public class GetSessionsRs : BaseResponse
    {
        public List<SessionDTO> Sessions { get; set; }
    }

    public class GetSessionsService : BaseAppService<GetSessionsRq, GetSessionsRs>
    {
        public GetSessionsService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<GetSessionsRs> RunAsync(GetSessionsRq rq)
        {
            List<SessionDTO> sessionDTOs = new List<SessionDTO>();

            DateTime start = rq.Start.Date;
            DateTime end = rq.Start.AddDays(7).AddSeconds(-1).Date;
            List<Session> sessions = await _unitOfWork.Sessions.ListAsync(x => x.Date <= end && x.Date >= start);

            if (sessions.Count > 0)
            {
                List<int> scheduleIds = sessions.Select(x => x.Id).ToList();
                List<Schedule> schedules = await _unitOfWork.Schedules.ListAsync(x => scheduleIds.Contains(x.Id));

                List<int> branchIds = schedules.Select(x => x.BranchId).ToList();
                List<Branch> branches = await _unitOfWork.Branches.ListAsync(x => branchIds.Contains(x.Id));

                List<int> trainerIds = schedules.Select(x => x.TrainerId).ToList();
                List<Trainer> trainers = await _unitOfWork.Trainers.ListAsync(x => trainerIds.Contains(x.Id));

                List<int> classIds = schedules.Select(x => x.ClassId).ToList();
                List<Class> classes = await _unitOfWork.Classes.ListAsync(x => classIds.Contains(x.Id));

                foreach (Session session in sessions)
                {
                    Schedule schedule = schedules.Single(x => x.Id == session.ScheduleId);
                    Branch branch = branches.Single(x => x.Id == schedule.BranchId);
                    Trainer trainer = trainers.Single(x => x.Id == schedule.TrainerId);
                    Class classs = classes.Single(x => x.Id == schedule.ClassId);

                    SessionDTO sessionDTO = new SessionDTO
                    {
                        Id = session.Id,
                        Date = session.Date,
                        Number = session.Number,
                        ScheduleId = session.ScheduleId,
                        Song = schedule.Song,
                        OpeningDate = schedule.OpeningDate,
                        DaysPerWeek = schedule.DaysPerWeek,
                        TotalSessions = sessions.Count(x => x.ScheduleId == schedule.Id),
                        StartTime = schedule.StartTime,
                        BranchId = schedule.BranchId,
                        BranchName = branch.Name,
                        ClassId = schedule.ClassId,
                        ClassName = classs.Name,
                        TrainerId = schedule.TrainerId,
                        TrainerName = trainer.Name
                    };

                    sessionDTOs.Add(sessionDTO);
                }
            }

            return new GetSessionsRs
            {
                Sessions = sessionDTOs
            };
        }
    }
}