using System;
using System.Collections.Generic;

namespace Application
{
    public class ScheduleDTO
    {
        public int? Id { get; set; }
        public string Song { get; set; }
        public DateTime OpeningDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public List<DayOfWeek> DaysPerWeek { get; set; } = new List<DayOfWeek>();
        public int? BranchId { get; set; }
        public int? ClassId { get; set; }
        public int? TrainerId { get; set; }
        public int? TotalSessions { get; set; }
    }
}