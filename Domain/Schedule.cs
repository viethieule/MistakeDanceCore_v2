using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace Domain
{
    public class Schedule : BaseEntity
    {
        // required by EF
        private Schedule()
        {
        }

        public Schedule(
            string song,
            DateTime openingDate,
            TimeSpan startTime,
            List<DayOfWeek> daysPerWeek,
            int branchId,
            int classId,
            int trainerId)
        {
            Song = song;
            OpeningDate = openingDate;
            StartTime = startTime;
            DaysPerWeek = daysPerWeek;
            BranchId = branchId;
            ClassId = classId;
            TrainerId = trainerId;
        }

        public string Song { get; private set; }
        public DateTime OpeningDate { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public List<DayOfWeek> DaysPerWeek { get; private set; } = new List<DayOfWeek>();
        public int BranchId { get; private set; }
        public int ClassId { get; private set; }
        public int TrainerId { get; private set; }

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