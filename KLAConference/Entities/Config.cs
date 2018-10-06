using NodaTime;
using System.Collections.Generic;

namespace KLAConference.Entities
{
    public class Config
    {
        public LocalTime StartTime { get; set; }

        public LocalTime EndTime { get; set; }

        public List<Break> Breaks { get; set; }

        /// <summary>
        /// Represents the total number of possible minutes of talks during the conference
        /// </summary>
        public long TalkTime { get; set; }
    }

    public class Break
    {
        public LocalTime StartTime { get; set; }
        public LocalTime EndTime { get; set; }
    }
}
