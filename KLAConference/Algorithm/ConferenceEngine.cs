using KLAConference.Entities;
using NodaTime;
using System.Collections.Generic;
using System.Linq;
using KLAConference.Infrastructure;
namespace KLAConference.Algorithm
{
    public class ConferenceEngine : IConferenceEngine
    {
        /* Assumptions:
         * This engine does not adjust break times if total talk minutes are greater than available time 
         * This engine takes all talks as input and does not support addition of talks one by one
         */

        /* Notes:
        * This engine is highly configurable using Config.json. System is configurable for
        *                                                       different break times as well as Start and End time of the conference
        */

        #region Fields        
        private List<Session> _sessions = new List<Session>();
        private Config _config;
        #endregion

        #region Constructor       
        public ConferenceEngine(Config config)
        {
            _config = config;
            CreateSessions(_config);
        }
        #endregion

        #region IConferenceEngine members        
        public ScheduledConference Run(IEnumerable<Talk> talks)
        {
            var result = new ScheduledConference();

            try
            {
                if (talks == null || !talks.Any())
                {
                    result.Type = ResultType.InvalidInput;
                    return result;
                }

                var totalTalkTime = talks.Sum(c => c.Duration);

                if (totalTalkTime > _config.TalkTime)
                {
                    result.Type = ResultType.MoreTalksComparedToAvailableTime;
                    return result;
                }
                else if (totalTalkTime < _config.TalkTime)
                {
                    result.Type = ResultType.NotEnoughTalksForTheConference;
                    return result;
                }
                else if (talks.Count() < _sessions.Count())
                {
                    result.Type = ResultType.OverlappingTalks;
                    return result;
                }
                else
                {
                    // Check whether any talk overlaps the session
                    if (IsTalksOverlappingSessions(talks))
                    {
                        result.Type = ResultType.OverlappingTalks;
                        return result;
                    }
                    result.Type = ResultType.Success;
                }
                
                for (int i = 0; i < _sessions.Count(); i++)
                {
                    // Clear previously scheduled talks if any
                    if (_sessions[i].Talks.Any())
                        _sessions[i].Talks.Clear();

                    var duration = _sessions[i].Duration;
                    var backTrackTalkId = -1;
                    var retryCount = 0;
                    while (!AddTalksToSession(talks, _sessions[i], backTrackTalkId))
                    {
                        // set the backTrackIndex, starting from first available talk
                        // Note: Retry using backtracking methodology
                        retryCount++;
                        backTrackTalkId = talks.Where(c => c.SessionId != 0).ElementAt(retryCount).Id;
                    }

                    // Assign sessionIds after successfully finding the Talks for the session
                    foreach (var talk in _sessions[i].Talks)
                    {
                        talk.SessionId = _sessions[i].Id;
                        //result.OrderedTalks.Add(talk);
                    }
                }
            }
            catch (System.Exception ex)
            {
                // Note: Should log the exception
                result.Type = ResultType.Exception;
            }

            _sessions = _sessions.OrderBy(c => c.Id).ToList();
            var prevTalk = default(Talk);

            foreach (var session in _sessions)
            {
                foreach (var currentTalk in session.Talks)
                {
                    if (!result.OrderedTalks.Any())
                    {
                        currentTalk.StartTime = _config.GetStartTime();
                        
                    }
                    else
                    {
                        currentTalk.StartTime = LocalTime.Add(prevTalk.StartTime, Period.FromMinutes(prevTalk.Duration));
                    }

                    result.OrderedTalks.Add(currentTalk);
                    prevTalk = currentTalk;
                }
            }

            return result;
        }

        private bool AddTalksToSession(IEnumerable<Talk> talks, Session currentSession, int backTrackTalkId)
        {
            var isSuccess = false;

            if(currentSession.Type == SessionType.Break)
            {
                var breakAsTalk = new Talk();
                breakAsTalk.Id = -1; // Distinguish the break as invalid Talk
                breakAsTalk.Duration = (currentSession.EndTime - currentSession.StartTime).Minutes;
                breakAsTalk.Name = $"Break ({breakAsTalk.Duration})";
                currentSession.Talks.Add(breakAsTalk);
                return true;
            }

            foreach (var talk in talks)
            {
                // Proceed only if talk is not allocated to any session
                if (talk.SessionId == 0 && talk.Id != backTrackTalkId && talk.Duration <= currentSession.Duration)
                {
                    if (currentSession.Duration - talk.Duration == 0)
                    {
                        // Found the talks for the session
                        currentSession.Talks.Add(talk);
                       // talk.SessionId = currentSession.Id;
                        isSuccess = true;
                        break;
                    }
                    else
                    {
                       // talk.SessionId = currentSession.Id;
                        currentSession.Duration -= talk.Duration;
                    }

                    currentSession.Talks.Add(talk);
                }
            }

            return isSuccess;
        }
        #endregion

        #region Private Methods       
        public void Load()
        {
            CreateSessions(_config);
        }

        /// <summary>
        /// Create configuration driven sessions
        /// </summary>
        /// <param name="config">Default config</param>
        private void CreateSessions(Config config)
        {
            var sessionId = 0;

            for (int i = 0; i < config.Breaks.Count(); i++)
            {
                sessionId++;
                // Handle first session
                if (i == 0)
                    AddSession(config.GetStartTime(), config.Breaks[i].GetStartTime(), sessionId, SessionType.Talk);
                else
                    AddSession(config.Breaks[i - 1].GetEndTime(), config.Breaks[i].GetStartTime(), sessionId, SessionType.Talk);

                // Add Break as a session
                AddSession(config.Breaks[i].GetStartTime(), config.Breaks[i].GetEndTime(), ++sessionId, SessionType.Break);

                // Handle last session
                if (i == config.Breaks.Count() - 1)
                {
                    AddSession(config.Breaks[i].GetEndTime(), config.GetEndTime(), ++sessionId, SessionType.Talk);
                }
            }

            config.TalkTime += _sessions.Sum(c => c.Duration);
            _sessions = _sessions.OrderBy(c => c.Duration).ToList();
        }

        private void AddSession(LocalTime startTime, LocalTime endTime, int sessionId, SessionType type)
        {
            Session session = new Session();
            session.Id = sessionId;
            session.StartTime = startTime;
            session.EndTime = endTime;
            session.Type = type;
            session.Duration = LocalTime.Subtract(session.EndTime, session.StartTime).TotalMinutes();
            _sessions.Add(session);
        }

        private bool IsTalksOverlappingSessions(IEnumerable<Talk> talks)
        {
            // Example: Sessions - 10, 5, 10 and talks are 8,7,10
            // Example: Sessions - 10, 5, 10 and talks are 23,1,1
            // These talks are overlapping sessions and no solution is possible

            var minSession = _sessions.Min(c => c.Duration);
            var maxSession = _sessions.Max(c => c.Duration);
            var minTalk = talks.Min(c => c.Duration);
            var maxTalk = talks.Max(c => c.Duration);

            return minSession < minTalk || maxTalk > maxSession;
        }
        #endregion
    }
}
