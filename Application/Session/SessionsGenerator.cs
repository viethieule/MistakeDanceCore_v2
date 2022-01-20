using System;
using System.Collections.Generic;
using Application.Interfaces;

namespace Application
{
    public class SessionsGenerator : ISessionsGenerator
    {
        public List<SessionDTO> Generate(ScheduleDTO schedule)
        {
            List<SessionDTO> sessions = new List<SessionDTO>();
            if (schedule.TotalSessions == 0)
            {
                return sessions;
            }

            List<DayOfWeek> daysPerWeek = schedule.DaysPerWeek;
            int firstSessionIndex = daysPerWeek.IndexOf(schedule.OpeningDate.DayOfWeek);
            DateTime sessionDate = schedule.OpeningDate;
            for (int i = firstSessionIndex, j = 1; i >= -1 && j > 0; i++, j++)
            {
                sessions.Add(new SessionDTO { Date = sessionDate, Number = j, ScheduleId = schedule.Id.Value });

                if (sessions.Count == schedule.TotalSessions)
                {
                    break;
                }

                if (i == daysPerWeek.Count - 1)
                {
                    sessionDate = sessionDate.AddDays(7 - (daysPerWeek[i] - daysPerWeek[0]));
                    i = -1;
                }
                else
                {
                    sessionDate = sessionDate.AddDays(daysPerWeek[i + 1] - daysPerWeek[i]);
                }
            }

            return sessions;
        }
    }
}