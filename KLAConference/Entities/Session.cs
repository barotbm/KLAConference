using NodaTime;
using System.Collections.Generic;

namespace KLAConference.Entities
{
    /// <summary>
    /// Represents particular time frame of the conference i.e. Morning session - 8 AM to 10:30 AM, Afternoon session - 10:45 to 2:30, 
    /// After Tea session - 2:45 to 4 PM
    /// </summary>
    public class Session
    {
        public int Id { get; set; }
        public LocalTime StartTime { get; set; }
        public LocalTime EndTime { get; set; }
        public long Duration { get; set; }
        public List<Talk> Talks { get; set; } = new List<Talk>(); // NULL object pattern - helps to avoid NULL referece exceptions

        /// <summary>
        /// Morning -> Afternoon -> After tea, this will help to order the talks from morning to evening
        /// </summary>
        public int Order { get; set; }
    }
}
