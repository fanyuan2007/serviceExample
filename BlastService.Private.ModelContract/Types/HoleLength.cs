namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// Request for HoleLength.
    /// </summary>
    public class HoleLength
    {
        /// <summary>
        /// Required.
        /// Must be greater than or equal to 0.0
        /// </summary>
        public double Total { get; set; }

        /// <summary>
        /// Required.
        /// Must be greater than or equal to 0.0
        /// </summary>
        public double Average { get; set; }
    }
}
