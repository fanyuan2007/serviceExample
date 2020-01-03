namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// Base class for all properties with Names
    /// Including:
    ///     Blast Project
    ///     Pattern
    ///     Hole
    /// </summary>
    public class NamedBaseProperty
    {
        /// <summary>
        /// Required
        /// </summary>
        public BaseProperty BaseProperties { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public string Name { get; set; }
    }
}
