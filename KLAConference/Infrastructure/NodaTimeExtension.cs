using NodaTime;

namespace KLAConference.Infrastructure
{
    public static class NodaTimeExtension
    {
        /// <summary>
        /// Converts Period to minutes
        /// </summary>
        /// <param name="localTime">Period</param>
        /// <returns>long - total minutes</returns>
        public static long TotalMinutes(this Period localTime)
        {
            // Note: Only supports Hours and Minitus 
            long totalMinutes = 0;
            totalMinutes += localTime.Hours * 60;
            totalMinutes += localTime.Minutes;
            return totalMinutes;
        }
    }
}
