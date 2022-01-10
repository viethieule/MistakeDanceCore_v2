using System;
using System.Collections.Generic;
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
    }
}