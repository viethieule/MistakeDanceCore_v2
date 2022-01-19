using System;
using System.Collections.Generic;
using Shared;

namespace Domain
{
    public class Session : BaseEntity
    {
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public int ScheduleId { get; set; }
        public List<Registration> Registrations { get; set; }

        public void SetNumber(int sessionNumber)
        {
            if (sessionNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sessionNumber));
            }
        }
    }
}