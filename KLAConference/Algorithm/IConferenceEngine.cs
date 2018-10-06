using KLAConference.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLAConference.Algorithm
{
    /// <summary>
    /// Interface driven engine. If we find better algorithm in the future, we can replace it with the existing one and no need to update the 
    /// dependencies 
    /// </summary>
    interface IConferenceEngine
    {
        ScheduledConference Run(IEnumerable<Talk> talks);
    }
}
