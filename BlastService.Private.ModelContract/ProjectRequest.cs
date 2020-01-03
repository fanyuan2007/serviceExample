namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// A request for a Project.
    /// </summary>
    public class ProjectRequest
    {
        /// <summary>
        /// Required
        /// </summary>
        public NamedBaseProperty NameBasedProperties { get; set; }

        /// <summary>
        /// Required.
        /// May be empty or whitespace.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public MeasurementUnit Unit { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public UTCTimeZone TimeZone { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public LocalTransformation LocalTransformation { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public CoordinateConvention? Mapping { get; set; }
    }
}
