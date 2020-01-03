using BlastService.Private.ModelContract;

using NetTopologySuite.Geometries;

using System;
using System.Collections.Generic;

namespace BlastService.Private.Models
{
    public class HoleResponse
    {
        public HoleResponse()
        {

        }
        // Database -> Response
        public HoleResponse(HoleDb holeDb)
        {
            Id = holeDb.Id;
            DrillPatternId = holeDb.DrillPatternId;
            BlastPatternId = holeDb.BlastPatternId;
            Name = holeDb.Name;
            CreatedAt = holeDb.CreatedAt;
            UpdatedAt = holeDb.UpdatedAt;
            AreaOfInfluence = holeDb.AreaOfInfluence;
            VolumeOfInfluence = holeDb.VolumeOfInfluence;
            HoleState = holeDb.HoleState;
            HoleUsage = holeDb.HoleUsage;
            LayoutType = holeDb.LayoutType;
            DesignDiameter = holeDb.DesignDiameter;
            DesignSubDrill = holeDb.DesignSubDrill;
            ValidationState = holeDb.ValidationState;
            DesignBenchCollar = holeDb.DesignBenchCollar;
            DesignBenchToe = holeDb.DesignBenchToe;
            Accuracy = holeDb.Accuracy;
            LengthAccuracy = holeDb.LengthAccuracy;
            DesignChargeWeight = holeDb.DesignChargeWeight;
            DesignChargeThickness = holeDb.DesignChargeThickness;
            ActualChargeWeight = holeDb.ActualChargeWeight;
            ActualChargeThickness = holeDb.ActualChargeThickness;
            ChargeTemplateName = holeDb.ChargeTemplateName;
            FragmentSize = holeDb.FragmentSize;
            PowderFactor = holeDb.PowderFactor;
            GeologyCode = holeDb.GeologyCode;

            if (holeDb.ActualFragmentation != null)
            {
                ActualFragmentation = new ActualFragmentation();
                ActualFragmentation.P10 = holeDb.ActualFragmentation.P10;
                ActualFragmentation.P20 = holeDb.ActualFragmentation.P20;
                ActualFragmentation.P30 = holeDb.ActualFragmentation.P30;
                ActualFragmentation.P40 = holeDb.ActualFragmentation.P40;
                ActualFragmentation.P50 = holeDb.ActualFragmentation.P50;
                ActualFragmentation.P60 = holeDb.ActualFragmentation.P60;
                ActualFragmentation.P70 = holeDb.ActualFragmentation.P70;
                ActualFragmentation.P80 = holeDb.ActualFragmentation.P80;
                ActualFragmentation.P90 = holeDb.ActualFragmentation.P90;
                ActualFragmentation.TopSize = holeDb.ActualFragmentation.TopSize;
            }

            DesignChargeProfile = new List<ChargeInterval>();
            ActualChargeProfile = new List<ChargeInterval>();
            foreach (var profileDb in holeDb.ChargeProfile)
            {
                var profile = new ChargeInterval();
                profile.From = profileDb.From;
                profile.To = profileDb.To;
                profile.Consumable = profileDb.Consumable;
                profile.Amount = profileDb.Amount;
                profile.Deck = profileDb.Deck;

                switch (profileDb.ProfileType)
                {
                    case ChargeProfileType.Actual:
                        ActualChargeProfile.Add(profile);
                        break;
                    case ChargeProfileType.Design:
                        DesignChargeProfile.Add(profile);
                        break;
                    default:
                        break;
                } 
            }

            #region Design Hole
            DesignHole = new HoleStructure();
            DesignHole.Azimuth = holeDb.DesignAzimuth;
            DesignHole.Dip = holeDb.DesignDip;
            DesignHole.Length = holeDb.DesignLength;

            DesignHole.Collar = new GPSCoordinate()
            {
                Longitude = holeDb.DesignCollar.X,
                Latitude = holeDb.DesignCollar.Y,
                Elevation = holeDb.DesignCollar.Z
            };
            DesignHole.Toe = new GPSCoordinate()
            {
                Longitude = holeDb.DesignToe.X,
                Latitude = holeDb.DesignToe.Y,
                Elevation = holeDb.DesignToe.Z
            };
            #endregion

            #region Actual Hole
            if (holeDb.HasActualStructure())
            {
                ActualAzimuth = holeDb.ActualAzimuth.Value;
                ActualDip = holeDb.ActualDip.Value;
                ActualLength = holeDb.ActualLength.Value;

                ActualCollar = new GPSCoordinate()
                {
                    Longitude = holeDb.ActualCollar.X,
                    Latitude = holeDb.ActualCollar.Y,
                    Elevation = holeDb.ActualCollar.Z
                };
                ActualToe = new GPSCoordinate()
                {
                    Longitude = holeDb.ActualToe.X,
                    Latitude = holeDb.ActualToe.Y,
                    Elevation = holeDb.ActualToe.Z
                };
            }
            #endregion

            #region Design Trace
            if (holeDb.DesignTrace != null)
            {
                var designs = new List<GPSCoordinate>();
                foreach (CoordinateZ point in holeDb.DesignTrace.Coordinates)
                {
                    if (point != null)
                    {
                        var coord = new GPSCoordinate()
                        {
                            Longitude = point.X,
                            Latitude = point.Y,
                            Elevation = point.Z
                        };

                        designs.Add(coord);
                    }
                }
                DesignTrace = designs;
            }
            #endregion

            #region Actual Trace
            if (holeDb.ActualTrace != null)
            {
                var actuals = new List<GPSCoordinate>();
                foreach (CoordinateZ point in holeDb.ActualTrace.Coordinates)
                {
                    if (point != null)
                    {
                        var coord = new GPSCoordinate()
                        {
                            Longitude = point.X,
                            Latitude = point.Y,
                            Elevation = point.Z
                        };

                        actuals.Add(coord);
                    }
                }
                ActualTrace = actuals;
            }
            #endregion
        }

        public Guid? DrillPatternId { get; set; }

        public Guid? BlastPatternId { get; set; }

        public ActualFragmentation ActualFragmentation { get; set; }

        public List<ChargeInterval> DesignChargeProfile { get; set; }

        public List<ChargeInterval> ActualChargeProfile { get; set; }

        #region Required Properties
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public double AreaOfInfluence { get; set; }

        public double VolumeOfInfluence { get; set; }

        public string HoleState { get; set; }

        public string HoleUsage { get; set; }

        public string LayoutType { get; set; }

        public double DesignDiameter { get; set; }

        public double DesignSubDrill { get; set; }

        public ValidationState ValidationState { get; set; }

        public HoleStructure DesignHole { get; set; }

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

        public IEnumerable<GPSCoordinate> DesignTrace { get; set; }

        public IEnumerable<GPSCoordinate> ActualTrace { get; set; }

        public double ActualAzimuth { get; set; }

        public double ActualDip { get; set; }

        public double ActualLength { get; set; }

        public GPSCoordinate ActualCollar { get; set; }

        public GPSCoordinate ActualToe { get; set; }
        #endregion
    }
}
