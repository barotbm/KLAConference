using System.Collections.Generic;

namespace KLAConference.Entities
{
    public class ScheduledConference
    {
        public ResultType Type { get; set; }

        public IList<Talk> OrderedTalks { get; set; } = new List<Talk>();
    }
}
