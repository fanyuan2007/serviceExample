namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// Request for a timezone offset.
    /// </summary>
    public class UTCTimeZone
    {
        /// <summary>
        /// The UTC offset in hours.
        /// Must be between -12 and 14 inclusive.
        /// Required.
        /// </summary>
        public int OffsetHours { get; set; }

        /// <summary>
        /// The UTC offset in minutes
        /// Must be between -59 and 59 inclusive.
        /// Required
        /// </summary>
        public int OffsetMinutes { get; set; }

        /// <summary>
        /// This is the UTC TimeZone Name, e.g. Pacific Standard Time
        /// Required. Must not be null, empty, or whitespaces.
        /// </summary>
        public string IdName { get; set; }

        /// <summary>
        /// Determines whether this UTC TimeZone is under daylight saving shift
        /// Required
        /// </summary>
        public bool IsDaylightSavingTime { get; set; }
    }
}