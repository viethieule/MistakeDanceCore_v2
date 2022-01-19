using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace Domain
{
    public class Schedule : BaseEntity
    {
        public string Song { get; set; }
        public DateTime OpeningDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public List<DayOfWeek> DaysPerWeek { get; set; } = new List<DayOfWeek>();
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
        public List<Session> Sessions { get; set; }

        public List<Session> GenerateSessions(int totalSessions)
        {
            List<Session> sessions = new List<Session>();
            if (totalSessions == 0)
            {
                return sessions;
            }

            int firstSessionIndex = DaysPerWeek.IndexOf(OpeningDate.DayOfWeek);
            DateTime sessionDate = OpeningDate;
            for (int i = firstSessionIndex, j = 1; i >= -1 && j > 0; i++, j++)
            {
                sessions.Add(new Session(sessionDate, j, Id));

                if (sessions.Count == totalSessions)
                {
                    break;
                }

                if (i == DaysPerWeek.Count - 1)
                {
                    sessionDate = sessionDate.AddDays(7 - (DaysPerWeek[i] - DaysPerWeek[0]));
                    i = -1;
                }
                else
                {
                    sessionDate = sessionDate.AddDays(DaysPerWeek[i + 1] - DaysPerWeek[i]);
                }
            }

            return sessions;
        }

        public void Update(Schedule newSchedule)
        {
            Song = newSchedule.Song;
            TrainerId = newSchedule.TrainerId;
            ClassId = newSchedule.ClassId;
            StartTime = newSchedule.StartTime;
            BranchId = newSchedule.BranchId;
            DaysPerWeek = newSchedule.DaysPerWeek;

            if (OpeningDate.Date != newSchedule.OpeningDate.Date && DateTime.Now > OpeningDate.Add(StartTime))
            {
                throw new Exception("ChangeOpeningDateOfStartedScheduleException");
            }
        }

        public bool ShouldUpdateSessions(Schedule newSchedule)
        {
            return OpeningDate != newSchedule.OpeningDate ||
                DaysPerWeek.Count != newSchedule.DaysPerWeek.Count ||
                !DaysPerWeek.All(x => newSchedule.DaysPerWeek.Contains(x));
        }
    }
}