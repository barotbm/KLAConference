using NodaTime;
using System;
using System.Collections.Generic;

namespace KLAConference.Entities
{
    public class Config
    {
        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public LocalTime GetStartTime()
        {
            return new LocalTime(StartTime, 0, 0);
        }

        public LocalTime GetEndTime()
        {
            return new LocalTime(EndTime, 0, 0);
        }

        public List<Break> Breaks { get; set; }

        /// <summary>
        /// Represents the total number of possible minutes of talks during the conference
        /// </summary>
        public long TalkTime { get; set; }
    }

    public class Break
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public LocalTime GetStartTime()
        {
            var dateTime = Convert.ToDateTime(StartTime);
            return new LocalTime(dateTime.Hour, dateTime.Minute, 0);
        }

        public LocalTime GetEndTime()
        {
            var dateTime = Convert.ToDateTime(EndTime);
            return new LocalTime(dateTime.Hour, dateTime.Minute, 0);
        }
    }
}
