namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// Request for ChargeInfo.
    /// </summary>
    public class ChargeInfo
    {
        /// <summary>
        /// Required
        /// If weight is specified, thickness is a function of SG of explosive, hole diameter and weight.
        /// Must be greater than or equal to 0.0
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Required
        /// If thickness is specified, weight is a function of SG of explosive, hole diameter and thickness.
        /// Must be greater than or equal to 0.0
        /// </summary>
        public double Thickness { get; set; }
    }
}