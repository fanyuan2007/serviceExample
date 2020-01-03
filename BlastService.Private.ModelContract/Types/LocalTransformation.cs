namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// Request for a LocalTransformation.
    /// </summary>
    public class LocalTransformation
    {
        /// <summary>
        /// Required
        /// </summary>
        public Transformation Translation { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public Transformation Rotation { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public Transformation Scaling { get; set; }
    }
}