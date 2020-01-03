using System;
using System.Collections.Generic;

namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// This class defines DTO properties for a Blast Pattern
    /// </summary>
    public class PatternRequest
    {
        /// <summary>
        /// Required
        /// </summary>
        public NamedBaseProperty NameBasedProperties { get; set; }

        /// <summary>
        /// Required.
        /// Must not be null, empty or whitespaces.
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Required.
        /// Must be between -90.0 and 90.0 inclusive.
        /// </summary>
        public double FaceAngle { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public double SubDrill { get; set; }

        /// <summary>
        /// Optional.
        /// If provided, must not be empty or only whitespaces.
        /// </summary>
        public string Bench { get; set; }

        /// <summary>
        /// Optional
        /// If provided, must not be empty or only whitespaces.
        /// </summary>
        public string Pit { get; set; }

        /// <summary>
        /// Optional
        /// If provided, must not be empty or only whitespaces.
        /// </summary>
        public string Phase { get; set; }

        /// <summary>
        /// Optional
        /// If provided, must be greater than 0.0
        /// </summary>
        public double? Area { get; set; }

        /// <summary>
        /// Optional
        /// If provided, must be greater than 0.0
        /// </summary>
        public double? Volume { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public HoleLength HoleLength { get; set; }

        /// <summary>
        /// Usage of Holes of this Pattern
        /// Required
        /// Must not be null, empty or whitespaces.
        /// </summary>
        public string HoleUsage { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public Scoring Scoring { get; set; }

        /// <summary>
        /// Optional.
        /// If provided, must not be empty or whitespaces.
        /// </summary>
        public string GeologyCode { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public PatternType PatternType { get; set; }

        /// <summary>
        /// Required.
        /// Must not be null, empty, or whitespaces.
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// Optional.
        /// If provided, must not be empty or whitespaces.
        /// </summary>
        public string PatternTemplateName { get; set; }

        /// <summary>
        /// Optional.
        /// If provided, must not be empty or whitespaces.
        /// </summary>
        public string ChargingTemplateName { get; set; }

        /// <summary>
        /// Determines whether this Pattern is electirc blast or not
        /// Required
        /// </summary>
        public bool IsElectronic { get; set; }

        /// <summary>
        /// Optional
        /// If provided, must be greater than or equal to 0.
        /// </summary>
        public int? MaxHoleFired { get; set; }

        /// <summary>
        /// Optional
        /// If provided, must be greater than or equal to 0.0
        /// </summary>
        public double? MaxWeightFired { get; set; }

        /// <summary>
        /// Required
        /// Must be greater than 0.0
        /// </summary>
        public double PowderFactor { get; set; }

        /// <summary>
        /// Required
        /// Must be greater than 0.0
        /// </summary>
        public double RockFactor { get; set; }

        /// <summary>
        /// Required
        /// Must be greater than 0.0
        /// </summary>
        public double RockSG { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public ValidationState ValidationState { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public DesignFragmentation DesignFragmentation { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public ActualFragmentation ActualFragmentation { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public IEnumerable<PolyGeom> DesignBoundary { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public IEnumerable<PolyGeom> ActualBoundary { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public IEnumerable<HoleRequest> Holes { get; set; }
    }
}
