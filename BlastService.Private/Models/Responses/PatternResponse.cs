using BlastService.Private.ModelContract;

using NetTopologySuite.Geometries;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BlastService.Private.Models
{
    public class PatternResponse
    {
        public PatternResponse()
        {

        }
        // Database -> Response
        public PatternResponse(PatternDb patternDb)
        {
            Id = patternDb.Id;
            Name = patternDb.Name;
            CreatedAt = patternDb.CreatedAt;
            UpdatedAt = patternDb.UpdatedAt;
            Stage = patternDb.Stage;
            FaceAngle = patternDb.FaceAngle;
            SubDrill = patternDb.SubDrill;
            HoleUsage = patternDb.HoleUsage;
            PatternType = patternDb.PatternType;
            Purpose = patternDb.Purpose;
            IsElectronic = patternDb.IsElectronic;
            PowderFactor = patternDb.PowderFactor;
            RockFactor = patternDb.RockFactor;
            RockSG = patternDb.RockSG;
            ValidationState = patternDb.ValidationState;
            Description = patternDb.Description;
            Bench = patternDb.Bench;
            Pit = patternDb.Pit;
            Phase = patternDb.Phase;
            Area = patternDb.Area;
            Volume = patternDb.Volume;
            GeologyCode = patternDb.GeologyCode;
            PatternTemplateName = patternDb.PatternTemplateName;
            ChargingTemplateName = patternDb.ChargingTemplateName;
            MaxHoleFired = patternDb.MaxHoleFired;
            MaxWeightFired = patternDb.MaxWeightFired;

            #region Hole Length
            if (patternDb.HasHoleLength())
            {
                HoleLength = new HoleLength()
                {
                    Total = patternDb.TotalHoleLength.Value,
                    Average = patternDb.AverageHoleLength.Value
                };
            }
            #endregion

            #region Scoring
            if (patternDb.HasScoring())
            {
                TotalScore = patternDb.TotalScore;
                MetricScores = patternDb.MetricScores;
            }
            #endregion

            #region Boundary
            DesignBoundary = new List<PolyGeom>();
            foreach (var geom in patternDb.DesignBoundary)
            {
                var polylines = geom as MultiLineString;
                if (polylines != null)
                {
                    var polyGeom = new PolyGeom();
                    polyGeom.PolyGeometry = new List<GPSCoordinate>();
                    List<GPSCoordinate> gpsCoords = new List<GPSCoordinate>();
                    foreach (LineString line in polylines.Geometries)
                    {
                        foreach (CoordinateZ point in line.Coordinates)
                        {
                            var coord = new GPSCoordinate()
                            {
                                Longitude = point.X,
                                Latitude = point.Y,
                                Elevation = point.Z
                            };
                            gpsCoords.Add(coord);
                        }
                    }
                    polyGeom.PolyGeometry = gpsCoords;
                    DesignBoundary.Add(polyGeom);
                }
            }

            if (patternDb.ActualBoundary != null)
            {
                ActualBoundary = new List<PolyGeom>();
                foreach (var geom in patternDb.ActualBoundary)
                {
                    var polylines = geom as MultiLineString;
                    if (polylines != null)
                    {
                        var polyGeom = new PolyGeom();
                        polyGeom.PolyGeometry = new List<GPSCoordinate>();
                        List<GPSCoordinate> gpsCoords = new List<GPSCoordinate>();
                        foreach (LineString line in polylines.Geometries)
                        {
                            foreach (CoordinateZ point in line.Coordinates)
                            {
                                var coord = new GPSCoordinate()
                                {
                                    Longitude = point.X,
                                    Latitude = point.Y,
                                    Elevation = point.Z
                                };
                                gpsCoords.Add(coord);
                            }
                        }
                        polyGeom.PolyGeometry = gpsCoords;
                        ActualBoundary.Add(polyGeom);
                    }
                }
            }
            #endregion

            #region Holes
            Holes = new List<HoleResponse>();
            var holesDb = patternDb.DrillHoles.Concat(patternDb.BlastHoles);
            foreach (var holeDb in holesDb)
            {
                var hole = new HoleResponse(holeDb);
                Holes.Add(hole);
            }
            #endregion

            #region Fragmentation
            if (patternDb.ActualFragmentation != null)
            {
                var actual = new ActualFragmentation
                {
                    P10 = patternDb.ActualFragmentation.P10,
                    P20 = patternDb.ActualFragmentation.P20,
                    P30 = patternDb.ActualFragmentation.P30,
                    P40 = patternDb.ActualFragmentation.P40,
                    P50 = patternDb.ActualFragmentation.P50,
                    P60 = patternDb.ActualFragmentation.P60,
                    P70 = patternDb.ActualFragmentation.P70,
                    P80 = patternDb.ActualFragmentation.P80,
                    P90 = patternDb.ActualFragmentation.P90,
                    TopSize = patternDb.ActualFragmentation.TopSize
                };
                ActualFragmentation = actual;
            }

            if (patternDb.DesignFragmentation != null)
            {
                var design = new DesignFragmentation
                {
                    P10 = patternDb.DesignFragmentation.P10,
                    P20 = patternDb.DesignFragmentation.P20,
                    P30 = patternDb.DesignFragmentation.P30,
                    P40 = patternDb.DesignFragmentation.P40,
                    P50 = patternDb.DesignFragmentation.P50,
                    P60 = patternDb.DesignFragmentation.P60,
                    P70 = patternDb.DesignFragmentation.P70,
                    P80 = patternDb.DesignFragmentation.P80,
                    P90 = patternDb.DesignFragmentation.P90
                };
                DesignFragmentation = design;
            }
            #endregion

            ProjectId = patternDb.ProjectId;
        }

        public List<HoleResponse> Holes { get; set; }

        public Guid ProjectId { get; set; }

        public DesignFragmentation DesignFragmentation { get; set; }

        public ActualFragmentation ActualFragmentation { get; set; }

        #region Required Properties
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Stage { get; set; }

        public double FaceAngle { get; set; }

        public double SubDrill { get; set; }

        public string HoleUsage { get; set; }

        public PatternType PatternType { get; set; }

        public string Purpose { get; set; }

        public bool IsElectronic { get; set; }

        public double PowderFactor { get; set; }

        public double RockFactor { get; set; }

        public double RockSG { get; set; }

        public ValidationState ValidationState { get; set; }

        public List<PolyGeom> DesignBoundary { get; set; }
        #endregion

        #region Optional Properties
        public string Description { get; set; }

        public string Bench { get; set; }

        public string Pit { get; set; }

        public string Phase { get; set; }

        public double? Area { get; set; }

        public double? Volume { get; set; }

        public HoleLength HoleLength { get; set; }

        public string GeologyCode { get; set; }

        public string PatternTemplateName { get; set; }

        public string ChargingTemplateName { get; set; }

        public int? MaxHoleFired { get; set; }

        public double? MaxWeightFired { get; set; }

        public double? TotalScore { get; set; }

        /// <summary>
        /// JSON string
        /// </summary>
        public string MetricScores { get; set; }

        public List<PolyGeom> ActualBoundary { get; set; }
        #endregion
    }
}
