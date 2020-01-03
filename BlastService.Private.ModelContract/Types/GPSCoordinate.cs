namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// This class defintes GPS Coordinate
    /// </summary>
    public class GPSCoordinate
    {
        /// <summary>
        /// Required.
        /// Must be between -90.0 and 90.0, non-inclusive.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Required.
        /// Must be between -180.0 and 180.0, non-inclusive.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Required.
        /// </summary>
        public double Elevation { get; set; }
    }
}
