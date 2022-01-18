using System;
using Shared;

namespace Domain
{
    public class Session : BaseEntity
    {
        public Session(DateTime date, int number, int scheduleId)
        {
            Date = date;
            Number = number;
            ScheduleId = scheduleId;
        }

        public DateTime Date { get; private set; }
        public int Number { get; private set; }
        public int ScheduleId { get; private set; }

        public void SetNumber(int sessionNumber)
        {
            if (sessionNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sessionNumber));
            }
        }
    }
}