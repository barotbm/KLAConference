using NodaTime;

namespace KLAConference.Entities
{
    /// <summary>
    /// Represents the entity for Talk 
    /// </summary>
    public class Talk
    {
        /// <summary>
        /// Associates talk with particular session and helps identify that this talk is already scheduled in the conference
        /// </summary>
        public int SessionId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public LocalTime StartTime { get; set; }

        public long Duration { get; set; }

        public  TalkType Type { get; set; }
    }
}
