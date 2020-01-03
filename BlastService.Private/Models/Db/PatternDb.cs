using BlastService.Private.ModelContract;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlastService.Private.Models
{
    [Table("Patterns")]
    public class PatternDb
    { 
        // For database migration
        public PatternDb()
        {
            BlastHoles = new List<HoleDb>();
            DrillHoles = new List<HoleDb>();
        }

        /// Exceptions:
        /// Request -> Database
        /// ArgumentException
        public PatternDb(Guid projectId, PatternRequest patternRequest)
            :this()
        {
            #region Base Properties
            Id = patternRequest.NameBasedProperties.BaseProperties.Id;
            Name = patternRequest.NameBasedProperties.Name;
            CreatedAt = patternRequest.NameBasedProperties.BaseProperties.CreatedAt;
            UpdatedAt = patternRequest.NameBasedProperties.BaseProperties.UpdatedAt;
            #endregion

            Stage = patternRequest.Stage;
            Description = patternRequest.Description;
            FaceAngle = patternRequest.FaceAngle;
            SubDrill = patternRequest.SubDrill;
            Bench = patternRequest.Bench;
            Pit = patternRequest.Pit;
            Phase = patternRequest.Phase;
            Area = patternRequest.Area;
            Volume = patternRequest.Volume;
            HoleUsage = patternRequest.HoleUsage;
            GeologyCode = patternRequest.GeologyCode;
            PatternType = patternRequest.PatternType;
            Purpose = patternRequest.Purpose;
            PatternTemplateName = patternRequest.PatternTemplateName;
            ChargingTemplateName = patternRequest.ChargingTemplateName;
            IsElectronic = patternRequest.IsElectronic;
            MaxHoleFired = patternRequest.MaxHoleFired;
            MaxWeightFired = patternRequest.MaxWeightFired;
            PowderFactor = patternRequest.PowderFactor;
            RockFactor = patternRequest.RockFactor;
            RockSG = patternRequest.RockSG;
            ValidationState = patternRequest.ValidationState;
            
            if (patternRequest.HoleLength != null)
            {
                TotalHoleLength = patternRequest.HoleLength.Total;
                AverageHoleLength = patternRequest.HoleLength.Average;
            }

            if (patternRequest.ActualFragmentation != null)
            {
                ActualFragmentation = new FragmentationDb(patternRequest.ActualFragmentation);
            }

            if (patternRequest.DesignFragmentation != null)
            {
                DesignFragmentation = new FragmentationDb(patternRequest.DesignFragmentation);
            }

            if (patternRequest.Holes != null)
            {
                foreach (var holeDT in patternRequest.Holes)
                {
                    var hole = new HoleDb(holeDT);
                    
                    // For a hole, DrillPattern is required
                    DrillHoles.Add(hole);
                    
                    if (hole.BlastPatternId.HasValue)
                    {
                        BlastHoles.Add(hole);
                    }
                }
            }

            #region Scoring
            if (patternRequest.Scoring != null)
            {
                TotalScore = patternRequest.Scoring.TotalScore;
                MetricScores = JsonConvert.SerializeObject(patternRequest.Scoring.MetricScore);
            }
            #endregion

            #region Boundaries

            var designs = new List<LineString>();
            foreach (var polyGeom in patternRequest.DesignBoundary)
            {
                var pointList = new List<CoordinateZ>();
                foreach (var coord in polyGeom.PolyGeometry)
                {
                    var p = new CoordinateZ(coord.Longitude, coord.Latitude, coord.Elevation);
                    pointList.Add(p);
                }

                var polyline = new LineString(pointList.ToArray());
                designs.Add(polyline);
            }
            DesignBoundary = new MultiLineString(designs.ToArray());

            if (patternRequest.ActualBoundary != null)
            {
                var actuals = new List<LineString>();
                foreach (var polyGeom in patternRequest.ActualBoundary)
                {
                    var pointList = new List<CoordinateZ>();
                    foreach (var coord in polyGeom.PolyGeometry)
                    {
                        var p = new CoordinateZ(coord.Longitude, coord.Latitude, coord.Elevation);
                        pointList.Add(p);
                    }

                    var polyline = new LineString(pointList.ToArray());
                    actuals.Add(polyline);
                }
                ActualBoundary = new MultiLineString(actuals.ToArray());
            }
            #endregion

            ProjectId = projectId;
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
        public string Stage { get; set; }

        [Required]
        public double FaceAngle { get; set; }

        [Required]
        public double SubDrill { get; set; }

        [Required]
        public string HoleUsage { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public PatternType PatternType { get; set; }

        [Required]
        public string Purpose { get; set; }

        [Required]
        public bool IsElectronic { get; set; }

        [Required]
        public double PowderFactor { get; set; }

        [Required]
        public double RockFactor { get; set; }

        [Required]
        public double RockSG { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public ValidationState ValidationState { get; set; }

        /// <summary>
        /// Force SQL column type to be geography for storing a list of 3D geometries.
        /// Need to remove this tag after migrating to Postgre
        /// </summary>
        [Required]
        [Column(TypeName = "geography")]
        public MultiLineString DesignBoundary { get; set; }
        #endregion

        #region Optional Properties
        public string Description { get; set; }

        public string Bench { get; set; }

        public string Pit { get; set; }

        public string Phase { get; set; }

        public double? Area { get; set; }

        public double? Volume { get; set; }

        public double? AverageHoleLength { get; set; }

        public double? TotalHoleLength { get; set; }

        public double? TotalScore { get; set; }

        public string GeologyCode { get; set; }

        public string PatternTemplateName { get; set; }

        public string ChargingTemplateName { get; set; }

        public int? MaxHoleFired { get; set; }

        public double? MaxWeightFired { get; set; }

        /// <summary>
        /// Metric Scores is a list saved as JSON format
        /// </summary>
        public string MetricScores { get; set; }

        /// <summary>
        /// Force SQL column type to be geography for storing a list of 3D geometries.
        /// </summary>
        [Column(TypeName = "geography")]
        public MultiLineString ActualBoundary { get; set; }
        #endregion
        
        #region Holes
        /// The Holes table has a FK constraint to Patterns table
        [InverseProperty("DrillPattern")]
        public List<HoleDb> DrillHoles { get; set; }

        /// The Holes table has a FK constraint to Patterns table
        [InverseProperty("BlastPattern")]
        public List<HoleDb> BlastHoles { get; set; }
        #endregion

        #region Project
        [Required]
        public Guid ProjectId { get; set; }
        /// <summary>
        /// No need to set this as it's used for table relationships
        /// The Patterns table has a FK constraint to Project table
        /// </summary>
        [ForeignKey("ProjectId")]
        public ProjectDb Project { get; set; }
        #endregion

        #region Fragmentations
        /// <summary>
        /// The Patterns table has a FK constraint to Fragmentation table
        /// </summary>
        public Guid? ActualFragmentId { get; set; }
        [ForeignKey("ActualFragmentId")]
        public FragmentationDb ActualFragmentation { get; set; }

        /// <summary>
        /// The Patterns table has a FK constraint to Fragmentation table
        /// </summary>
        public Guid? DesignFragmentId { get; set; }
        [ForeignKey("DesignFragmentId")]
        public FragmentationDb DesignFragmentation { get; set; }
        #endregion

        #region Public Methods

        /// <summary>
        /// A pattern contains hole length if and only if both TotalHoleLength and AverageHoleLength exist
        /// </summary>
        /// <returns></returns>
        public bool HasHoleLength()
        {
            return TotalHoleLength.HasValue && AverageHoleLength.HasValue;
        }

        /// <summary>
        /// A pattern contains scoring if and only if both TotalScore and MetricScores exist
        /// </summary>
        /// <returns></returns>
        public bool HasScoring()
        {
            return TotalScore.HasValue && MetricScores != null;
        }

        #endregion
    }
}
