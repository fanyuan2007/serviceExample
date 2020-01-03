namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// Base class for DesignFragmentation and ActualFragmentation.
    /// </summary>
    public class Fragmentation
    {
        /// <summary>
        /// Optional. If present, value must be greater than 0.0
        /// At least one P-value must be non-null.
        /// </summary>
        public double? P10 { get; set; }

        /// <summary>
        /// Optional. If present, value must be greater than 0.0
        /// At least one P-value must be non-null.
        /// </summary>
        public double? P20 { get; set; }

        /// <summary>
        /// Optional. If present, value must be greater than 0.0
        /// At least one P-value must be non-null.
        /// </summary>
        public double? P30 { get; set; }

        /// <summary>
        /// At least one P-value must be non-null.
        /// Optional. If present, value must be greater than 0.0
        /// </summary>
        public double? P40 { get; set; }

        /// <summary>
        /// Optional. If present, value must be greater than 0.0
        /// At least one P-value must be non-null.
        /// </summary>
        public double? P50 { get; set; }

        /// <summary>
        /// Optional. If present, value must be greater than 0.0
        /// At least one P-value must be non-null.
        /// </summary>
        public double? P60 { get; set; }

        /// <summary>
        /// Optional. If present, value must be greater than 0.0
        /// At least one P-value must be non-null.
        /// </summary>
        public double? P70 { get; set; }

        /// <summary>
        /// Optional. If present, value must be greater than 0.0
        /// At least one P-value must be non-null.
        /// </summary>
        public double? P80 { get; set; }

        /// <summary>
        /// Optional. If present, value must be greater than 0.0
        /// At least one P-value must be non-null.
        /// </summary>
        public double? P90 { get; set; }
    }

    /// <summary>
    /// Class representing DesignFragmentation.
    /// </summary>
    public class DesignFragmentation : Fragmentation
    {
        // Has no additional properties.
    }

    /// <summary>
    /// Class representing ActualFragmentation.
    /// </summary>
    public class ActualFragmentation : Fragmentation
    {
        /// <summary>
        /// Optional. If present, value must be greater than 0.0
        /// At least one P-value must be non-null, or TopSize.
        /// </summary>
        public double? TopSize { get; set; }
    }
}