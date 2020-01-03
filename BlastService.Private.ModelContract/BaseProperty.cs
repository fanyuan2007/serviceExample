using System;

namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// Base class for all properties
    /// Including:
    ///     Blast Project
    ///     Pattern
    ///     Hole
    ///     Charging Interval
    /// </summary>
    public class BaseProperty
    {
        /// <summary>
        /// Required
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Required. Must not be later than the UpdatedAt date.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Required. Must not be earlier than the CreatedAt date.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
