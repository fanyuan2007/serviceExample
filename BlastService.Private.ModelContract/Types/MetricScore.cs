namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// Request for a MetricScore
    /// </summary>
    public class MetricScore
    {
        /// <summary>
        /// Required.
        /// Must not be null, empty or only whitespaces.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Required.
        /// Must be greater than 0.0
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public double Score { get; set; }
    }
}
