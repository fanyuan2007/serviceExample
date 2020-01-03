using System;
using System.Collections.Generic;

namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// This class defines DTO properties for a Blast Hole
    /// </summary>
    public class HoleRequest
    {
        /// <summary>
        /// Required
        /// </summary>
        public NamedBaseProperty NameBasedProperties { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public Guid DrillPatternId { get; set; }

        /// <summary>
        /// Id of the Blast Pattern the Hole belongs to
        /// Optional
        /// </summary>
        public Guid? BlastPatternId { get; set; }

        /// <summary>
        /// Required.
        /// Must be greater than 0.0
        /// </summary>
        public double AreaOfInfluence { get; set; }

        /// <summary>
        /// Required.
        /// Must be greater than 0.0
        /// </summary>
        public double VolumeOfInfluence { get; set; }

        /// <summary>
        /// Required.
        /// Must not be null, empty or only whitespaces.
        /// </summary>
        public string HoleState { get; set; }

        /// <summary>
        /// Required.
        /// Must not be null, empty or only whitespaces.
        /// </summary>
        public string HoleUsage { get; set; }

        /// <summary>
        /// Required
        /// Must not be null, empty or only whitespaces.
        /// </summary>
        public string LayoutType { get; set; }

        /// <summary>
        /// Required.
        /// Must be greater than or equal to 0.0.
        /// </summary>
        public double DesignDiameter { get; set; }

        /// <summary>
        /// Optional.
        /// </summary>
        public double? DesignBenchCollar { get; set; }

        /// <summary>
        /// Optional.
        /// </summary>
        public double? DesignBenchToe { get; set; }

        /// <summary>
        /// Required.
        /// </summary>
        public double DesignSubDrill { get; set; }

        /// <summary>
        /// Optional.
        /// </summary>
        public double? Accuracy { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public double? LengthAccuracy { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public ChargeInfo DesignChargeInfo { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public ChargeInfo ActualChargeInfo { get; set; }

        /// <summary>
        /// Optional.
        /// If provided, must not be empty or only whitespaces.
        /// </summary>
        public string ChargeTemplateName { get; set; }

        /// <summary>
        /// Optional. If provided, must be greater than 0.0
        /// </summary>
        public double? FragmentSize { get; set; }

        /// <summary>
        /// Optional. If provided, must be greater than 0.0
        /// </summary>
        public double? PowderFactor { get; set; }

        /// <summary>
        /// Optional. If provided, must not be empty or only whitespaces.
        /// </summary>
        public string GeologyCode { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public ValidationState ValidationState { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public ActualFragmentation ActualFragmentation { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public IEnumerable<ChargeInterval> DesignChargeProfile { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public IEnumerable<ChargeInterval> ActualChargeProfile { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public HoleStructure DesignHole { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public HoleStructure ActualHole { get; set; }

        /// <summary>
        /// Optional
        /// If not null, must have at least two coordinates.
        /// </summary>
        public IEnumerable<GPSCoordinate> DesignTrace { get; set; }

        /// <summary>
        /// Optional
        /// If not null, must have at least two coordinates.
        /// </summary>
        public IEnumerable<GPSCoordinate> ActualTrace { get; set; }
    }
}
