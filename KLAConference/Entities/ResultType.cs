namespace KLAConference.Entities
{
    public enum ResultType
    {
        None,
        InvalidInput,
        Success,
        OverlappingTalks, // i.e. One talk of duration 7.5 hours for all sessions or two talks of 3 hours each and one talk of 1.5 hour
        NotEnoughTalksForTheConference, // i.e. Total time of talks is less than 7.5 hours (4 to 8 with 30 mins break = 7.5 hrs)
        MoreTalksComparedToAvailableTime, // i.e. Total time of talks is more than 7.5 hours
        Exception
    }
}
