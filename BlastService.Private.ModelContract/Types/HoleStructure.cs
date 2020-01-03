namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// Request for a HoleStructure.
    /// </summary>
    public class HoleStructure
    {
        /// <summary>
        /// Required.
        /// Must be between 0.0 and 360.0 inclusive.
        /// </summary>
        public double Azimuth { get; set; }

        /// <summary>
        /// Required.
        /// Must be between -90.0 and 90.0 inclusive.
        /// </summary>
        public double Dip { get; set; }

        /// <summary>
        /// Required.
        /// Must be greater than or equal to 0.0
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Required.
        /// </summary>
        public GPSCoordinate Collar { get; set; }

        /// <summary>
        /// Required.
        /// </summary>
        public GPSCoordinate Toe { get; set; }
    }
}
