using BlastService.Private.ModelContract;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlastService.Private.Models
{
    [Table("Holes")]
    public class HoleDb
    {
        // For database migration
        public HoleDb()
        {
            ChargeProfile = new List<ChargeIntervalDb>();
        }

        /// Exceptions:
        /// Request -> Database
        public HoleDb(HoleRequest holeRequest)
            : this()
        {
            #region Base Properties
            Id = holeRequest.NameBasedProperties.BaseProperties.Id;
            Name = holeRequest.NameBasedProperties.Name;
            CreatedAt = holeRequest.NameBasedProperties.BaseProperties.CreatedAt;
            UpdatedAt = holeRequest.NameBasedProperties.BaseProperties.UpdatedAt;
            #endregion

            DrillPatternId = holeRequest.DrillPatternId;
            BlastPatternId = holeRequest.BlastPatternId;
            AreaOfInfluence = holeRequest.AreaOfInfluence;
            VolumeOfInfluence = holeRequest.VolumeOfInfluence;
            HoleState = holeRequest.HoleState;
            HoleUsage = holeRequest.HoleUsage;
            LayoutType = holeRequest.LayoutType;
            DesignDiameter = holeRequest.DesignDiameter;
            DesignBenchCollar = holeRequest.DesignBenchCollar;
            DesignBenchToe = holeRequest.DesignBenchToe;
            DesignSubDrill = holeRequest.DesignSubDrill;
            Accuracy = holeRequest.Accuracy;
            LengthAccuracy = holeRequest.LengthAccuracy;

            #region Charge Info
            if (holeRequest.DesignChargeInfo != null)
            {
                DesignChargeThickness = holeRequest.DesignChargeInfo.Thickness;
                DesignChargeWeight = holeRequest.DesignChargeInfo.Weight;
            }

            if (holeRequest.ActualChargeInfo != null)
            {
                ActualChargeThickness = holeRequest.ActualChargeInfo.Thickness;
                ActualChargeWeight = holeRequest.ActualChargeInfo.Weight;
            }
            #endregion

            ChargeTemplateName = holeRequest.ChargeTemplateName;
            FragmentSize = holeRequest.FragmentSize;
            PowderFactor = holeRequest.PowderFactor;
            GeologyCode = holeRequest.GeologyCode;
            ValidationState = holeRequest.ValidationState;

            if (holeRequest.ActualFragmentation != null)
            {
                ActualFragmentation = new FragmentationDb(holeRequest.ActualFragmentation);
                ActualFragmentId = ActualFragmentation.Id;
            }

            #region Charge Profile
            foreach (var profileRequest in holeRequest.DesignChargeProfile)
            {
                var profile = new ChargeIntervalDb(profileRequest);
                profile.HoleId = this.Id;
                profile.ProfileType = ChargeProfileType.Design;
                ChargeProfile.Add(profile);
            }

            foreach (var profileRequest in holeRequest.ActualChargeProfile)
            {
                var profile = new ChargeIntervalDb(profileRequest);
                profile.HoleId = this.Id;
                profile.ProfileType = ChargeProfileType.Actual;
                ChargeProfile.Add(profile);
            }
            #endregion

            #region Hole Structure
            DesignAzimuth = holeRequest.DesignHole.Azimuth;
            DesignDip = holeRequest.DesignHole.Dip;
            DesignLength = holeRequest.DesignHole.Length;
            // Geometry Point is presented in the following order: longtitude, latitude, and elevation
            DesignCollar = new Point(holeRequest.DesignHole.Collar.Longitude, 
                                    holeRequest.DesignHole.Collar.Latitude, 
                                    holeRequest.DesignHole.Collar.Elevation);
            DesignToe = new Point(holeRequest.DesignHole.Toe.Longitude,
                                  holeRequest.DesignHole.Toe.Latitude,
                                  holeRequest.DesignHole.Toe.Elevation);

            if (holeRequest.ActualHole != null)
            {
                ActualAzimuth = holeRequest.ActualHole.Azimuth;
                ActualDip = holeRequest.ActualHole.Dip;
                ActualLength = holeRequest.ActualHole.Length;
                // Geometry Point is presented in the following order: longtitude, latitude, and elevation
                ActualCollar = new Point(holeRequest.ActualHole.Collar.Longitude,
                                        holeRequest.ActualHole.Collar.Latitude,
                                        holeRequest.ActualHole.Collar.Elevation);
                ActualToe = new Point(holeRequest.ActualHole.Toe.Longitude,
                                      holeRequest.ActualHole.Toe.Latitude,
                                      holeRequest.ActualHole.Toe.Elevation);
            }
            #endregion

            #region Traces
            if (holeRequest.DesignTrace != null)
            {
                var points = new List<CoordinateZ>();
                foreach (var coord in holeRequest.DesignTrace)
                {
                    var p = new CoordinateZ(coord.Longitude, coord.Latitude, coord.Elevation);
                    points.Add(p);
                }
                DesignTrace = new LineString(points.ToArray());
            }

            if (holeRequest.ActualTrace != null)
            {
                var points = new List<CoordinateZ>();
                foreach (var coord in holeRequest.ActualTrace)
                {
                    var p = new CoordinateZ(coord.Longitude, coord.Latitude, coord.Elevation);
                    points.Add(p);
                }
                ActualTrace = new LineString(points.ToArray());
            }
            #endregion
        }

        #region Required Properties
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public double AreaOfInfluence { get; set; }

        [Required]
        public double VolumeOfInfluence { get; set; }

        [Required]
        public string HoleState { get; set; }

        [Required]
        public string HoleUsage { get; set; }

        [Required]
        public string LayoutType { get; set; }

        [Required]
        public double DesignDiameter { get; set; }

        [Required]
        public double DesignSubDrill { get; set; }

        [Required]
        public ValidationState ValidationState { get; set; }

        [Required]
        public double DesignAzimuth { get; set; }

        [Required]
        public double DesignDip { get; set; }

        [Required]
        public double DesignLength { get; set; }

        /// <summary>
        /// Force SQL column type to be geography for storing 3D coordinates.
        /// </summary>
        [Required]
        [Column(TypeName = "geography")]
        public Point DesignCollar { get; set; }

        /// <summary>
        /// Force SQL column type to be geography for storing 3D coordinates
        /// </summary>
        [Required]
        [Column(TypeName = "geography")]
        public Point DesignToe { get; set; }
        #endregion

        #region Optional Properties
        public double? DesignBenchCollar { get; set; }

        public double? DesignBenchToe { get; set; }

        public double? Accuracy { get; set; }

        public double? LengthAccuracy { get; set; }

        public double? DesignChargeWeight { get; set; }

        public double? DesignChargeThickness { get; set; }

        public double? ActualChargeWeight { get; set; }

        public double? ActualChargeThickness { get; set; }

        public string ChargeTemplateName { get; set; }

        public double? FragmentSize { get; set; }

        public double? PowderFactor { get; set; }

        public string GeologyCode { get; set; }

        /// <summary>
        /// Force SQL column type to be geography for storing a list of 3D coordinates.
        /// </summary>
        [Column(TypeName = "geography")]
        public LineString DesignTrace { get; set; }

        /// <summary>
        /// Force SQL column type to be geography for storing a list of 3D coordinates.
        /// </summary>
        [Column(TypeName = "geography")]
        public LineString ActualTrace { get; set; }

        public double? ActualAzimuth { get; set; }

        public double? ActualDip { get; set; }

        public double? ActualLength { get; set; }

        /// <summary>
        /// Force SQL column type to be geography for storing 3D coordinates.
        /// </summary>
        [Column(TypeName = "geography")]
        public Point ActualCollar { get; set; }

        /// <summary>
        /// Force SQL column type to be geography for storing 3D coordinates
        /// </summary>
        [Column(TypeName = "geography")]
        public Point ActualToe { get; set; }
        #endregion

        #region Patterns
        [Required]
        public Guid DrillPatternId { get; set; }
        /// <summary>
        /// The Holes table has a FK constraint to Patterns table
        /// No need to set this as it's used for table relationships
        /// </summary>
        
        [ForeignKey("DrillPatternId")]
        public PatternDb DrillPattern { get; set; }

        /// <summary>
        /// The Holes table has a FK constraint to Patterns table
        /// </summary>
        public Guid? BlastPatternId { get; set; }
        /// <summary>
        /// No need to set this as it's used for table relationships
        /// </summary>
        [ForeignKey("BlastPatternId")]
        public PatternDb BlastPattern { get; set; }
        #endregion

        #region Fragmentations
        /// <summary>
        /// The Holes table has a FK constraint to Fragmentation table
        /// </summary>
        public Guid? ActualFragmentId { get; set; }
        [ForeignKey("ActualFragmentId")]
        public FragmentationDb ActualFragmentation { get; set; }
        #endregion

        #region Charging Profiles
        /// <summary>
        /// The Charging table has a FK constraints to Holes table
        /// </summary>
        public List<ChargeIntervalDb> ChargeProfile { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// A hole contains actual hole structure if and only if azimuth, dip, length, collar, and toe all exist.
        /// </summary>
        /// <returns></returns>
        public bool HasActualStructure()
        {
            return ActualAzimuth.HasValue && ActualDip.HasValue && ActualLength.HasValue &&
                   ActualCollar != null && ActualToe != null;
        }

        #endregion
    }
}
