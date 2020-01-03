namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// Request for ChargeInterval.
    /// </summary>
    public class ChargeInterval
    {
        /// <summary>
        /// Required.
        /// Must be greater than or equal to 0.0
        /// Must not be greater than To.
        /// </summary>
        public double From { get; set; }

        /// <summary>
        /// Required
        /// Must be greater than or equal to 0.0
        /// Must not be smaller than From.
        /// </summary>
        public double To { get; set; }

        /// <summary>
        /// Required
        /// Must not be null, empty, or whitespaces.
        /// </summary>
        public string Consumable { get; set; }

        /// <summary>
        /// Required
        /// Must be greater than or equal to 0.0
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Optional.
        /// If provided, must not be whitespaces.
        /// </summary>
        public string Deck { get; set; }
    }
}