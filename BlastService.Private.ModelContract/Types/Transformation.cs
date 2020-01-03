namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// A Transformation request.
    /// </summary>
    public class Transformation
    {
        /// <summary>
        /// Required.
        /// Must be a valid double.
        /// </summary>
        public double East { get; set; }

        /// <summary>
        /// Required.
        /// Must be a valid double.
        /// </summary>
        public double North { get; set; }

        /// <summary>
        /// Required.
        /// Must be a valid double.
        /// </summary>
        public double Elev { get; set; }
    }
}