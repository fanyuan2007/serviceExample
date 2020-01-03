using System;
using System.Collections.Generic;

using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class PatternValidationTest
    {
        private PatternRequest _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = CreateSamplePatternRequest();

            _validator = RequestValidatorFactory.CreateValidator();
        }

        [TearDown]
        public void TearDown()
        {
            _request = null;
            _validator = null;
        }

        [Test]
        public void NameBasedPropertiesTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // NOT null
            _request.NameBasedProperties = null; 
            Assert.IsFalse(_validator.Validate(_request, out errors));
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void StageTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Stage = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(Double.NaN, false)]
        [TestCase(-91.0, false)]
        [TestCase(-90.0, true)]
        [TestCase(-89.0, true)]
        [TestCase(89.0, true)]
        [TestCase(90.0, true)]
        [TestCase(91.0, false)]
        public void FaceAngleTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.FaceAngle = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
        
        [TestCase(Double.NaN, false)]
        public void SubDrillTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.SubDrill = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void BenchTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Bench = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void PitTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Pit = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void PhaseTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Phase = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(null, true)]
        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(1.0, true)]
        public void AreaTest(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Area = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, true)]
        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(1.0, true)]
        public void VolumeTest(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Volume = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void HoleUsageTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.HoleUsage = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ScoringTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Null is okay.
            _request.Scoring = null;
            Assert.IsTrue(_validator.Validate(_request, out errors));
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void GeologyCodeTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.GeologyCode = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void PurposeTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Purpose = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void PatternTemplateNameTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.PatternTemplateName = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void ChargingTemplateNameTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.ChargingTemplateName = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, true)]
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        public void MaxHoleFiredTest(int? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.MaxHoleFired = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(null, true)]
        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(1.0, true)]
        public void MaxWeightFiredTest(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.MaxWeightFired = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(1.0, true)]
        public void PowderFactorTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.PowderFactor = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
        
        // Double comparison
        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(1.0, true)]
        public void RockFactorTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.RockFactor = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(1.0, true)]
        public void RockSGTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.RockSG = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void DesignFragmentationTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Null
            _request.DesignFragmentation = null;
            Assert.IsTrue(_validator.Validate(_request, out errors));
        }

        [Test]
        public void ActualFragmentationTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Null
            _request.ActualFragmentation = null;
            Assert.IsTrue(_validator.Validate(_request, out errors));
        }

        [Test]
        public void DesignBoundaryTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Must not be null.
            _request.DesignBoundary = null;
            Assert.IsFalse(_validator.Validate(_request, out errors));

            // Must have at least one polygon.
            var list = new List<PolyGeom>();
            _request.DesignBoundary = list;
            Assert.IsFalse(_validator.Validate(_request, out errors));

            list.Add(new PolyGeom()
            {
                PolyGeometry = new List<GPSCoordinate>()
                {
                    new GPSCoordinate()
                    {
                        Latitude = 0,
                        Longitude = 1,
                        Elevation = 2,
                    },
                    new GPSCoordinate()
                    {
                        Latitude = 3,
                        Longitude = 4,
                        Elevation = 5,
                    },
                    new GPSCoordinate()
                    {
                        Latitude = 6,
                        Longitude = 7,
                        Elevation = 8,
                    }
                }
            });

            Assert.IsTrue(_validator.Validate(_request, out errors));
        }
        [Test]
        public void ActualBoundaryTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Can be null.
            _request.ActualBoundary = null;
            Assert.IsTrue(_validator.Validate(_request, out errors));

            // If not null, must have at least one item.
            var list = new List<PolyGeom>();
            _request.ActualBoundary = list;
            Assert.IsFalse(_validator.Validate(_request, out errors));

            list.Add(new PolyGeom()
            {
                PolyGeometry = new List<GPSCoordinate>()
                {
                    new GPSCoordinate()
                    {
                        Latitude = 0,
                        Longitude = 1,
                        Elevation = 2,
                    },
                    new GPSCoordinate()
                    {
                        Latitude = 3,
                        Longitude = 4,
                        Elevation = 5,
                    },
                    new GPSCoordinate()
                    {
                        Latitude = 6,
                        Longitude = 7,
                        Elevation = 8,
                    }
                }
            });

            Assert.IsTrue(_validator.Validate(_request, out errors));
        }

        [Test]
        public void HolesTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Null
            _request.Holes = null;
            Assert.IsTrue(_validator.Validate(_request, out errors));

            // Must have at least one HoleRequest if not null.
            var holes = new List<HoleRequest>();
            _request.Holes = holes;
            Assert.IsFalse(_validator.Validate(_request, out errors));

            // Can't just instantiate a default HoleRequest :(
            // Needs to be a fully instantiated one as there are required values.
            var hole = CreateSampleHoleRequest();
            holes.Add(hole);

            Assert.IsTrue(_validator.Validate(_request, out errors));
        }

        #region Helper Functions
        private HoleRequest CreateSampleHoleRequest()
        {
            var request = new HoleRequest()
            {
                NameBasedProperties = new NamedBaseProperty()
                {
                    BaseProperties = new BaseProperty()
                    {
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                    },
                    Name = "TestHole",
                },

                DrillPatternId = new Guid(),
                BlastPatternId = new Guid(),
                AreaOfInfluence = 10.0,
                VolumeOfInfluence = 10.0,
                HoleState = "Blasted",
                HoleUsage = "SampleSite",
                LayoutType = "Foo",
                DesignDiameter = 5.0,
                DesignBenchCollar = 25.0,
                DesignBenchToe = 50.0,
                DesignSubDrill = 30.0,
                Accuracy = 10.0,
                LengthAccuracy = 10.0,
                DesignChargeInfo = new ChargeInfo()
                {
                    Thickness = 5.0,
                    Weight = 5.0
                },
                ActualChargeInfo = new ChargeInfo()
                {
                    Thickness = 5.0,
                    Weight = 5.0
                },
                ChargeTemplateName = "Bar",
                FragmentSize = 5.0,
                PowderFactor = 5.0,
                GeologyCode = "LITO",
                ValidationState = ValidationState.Valid,
                ActualFragmentation = new ActualFragmentation()
                {
                    P10 = 1.0,
                    P20 = 2.0,
                    P30 = 3.0,
                    P40 = 4.0,
                    P50 = 5.0,
                    P60 = 6.0,
                    P70 = 7.0,
                    P80 = 8.0,
                    P90 = 9.0,
                    TopSize = 999.00
                },
                DesignChargeProfile = new List<ChargeInterval>()
                {
                    new ChargeInterval() { Amount = 20, Consumable = "Foo", Deck = "Bar", From = 0.0, To = 4.0 },
                },
                ActualChargeProfile = new List<ChargeInterval>()
                {
                    new ChargeInterval() { Amount = 20, Consumable = "Foo", Deck = "Bar", From = 0.0, To = 4.0 },
                },
                DesignHole = new HoleStructure()
                {
                    Azimuth = 15.00,
                    Dip = -45.00,
                    Length = 5.00,
                    Collar = new GPSCoordinate()
                    {
                        Latitude = 15.25,
                        Longitude = 67.62,
                        Elevation = 10.00,
                    },
                    Toe = new GPSCoordinate()
                    {
                        Latitude = 20.00,
                        Longitude = 68.65,
                        Elevation = 100.00,
                    }
                },
                ActualHole = new HoleStructure()
                {
                    Azimuth = 15.00,
                    Dip = -45.00,
                    Length = 5.00,
                    Collar = new GPSCoordinate()
                    {
                        Latitude = 15.25,
                        Longitude = 67.62,
                        Elevation = 10.00,
                    },
                    Toe = new GPSCoordinate()
                    {
                        Latitude = 20.00,
                        Longitude = 68.65,
                        Elevation = 100.00,
                    }
                },
                DesignTrace = new List<GPSCoordinate>()
                {
                    new GPSCoordinate()
                    {
                        Latitude = 5,
                        Longitude = 5,
                        Elevation = 5,
                    },
                    new GPSCoordinate()
                    {
                        Latitude = 10,
                        Longitude = 10,
                        Elevation = 10,
                    }
                },
                ActualTrace = new List<GPSCoordinate>()
                {
                    new GPSCoordinate()
                    {
                        Latitude = 5,
                        Longitude = 5,
                        Elevation = 5,
                    },
                    new GPSCoordinate()
                    {
                        Latitude = 10,
                        Longitude = 10,
                        Elevation = 10,
                    }
                },
            };

            return request;
        }

        private PatternRequest CreateSamplePatternRequest()
        {
            var request = new PatternRequest()
            {
                NameBasedProperties = new NamedBaseProperty()
                {
                    BaseProperties = new BaseProperty()
                    {
                        CreatedAt = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                        UpdatedAt = DateTime.UtcNow,
                    },
                    Name = "Foo",
                },
                Stage = "Blasted",
                Description = "The quick brown fox jumped over the lazy dog",
                FaceAngle = 45.0,
                SubDrill = 5,
                Bench = "One",
                Pit = "Two",
                Phase = "Three",
                Area = 15.0,
                Volume = 25.0,
                HoleLength = new HoleLength()
                {
                    Average = 5.0,
                    Total = 25.0,
                },
                HoleUsage = "Test",
                Scoring = new Scoring()
                {
                    MetricScore = new List<MetricScore>()
                    {
                        new MetricScore()
                        {
                            Name = "TestScore",
                            Score = 4,
                            Weight = 2.25,
                        }
                    },
                    TotalScore = 4.5
                },
                GeologyCode = "LITO",
                PatternType = PatternType.Both,
                Purpose = "TestPurpose",
                PatternTemplateName = "SomeTemplate",
                ChargingTemplateName = "AnotherTemplate",
                IsElectronic = true,
                MaxHoleFired = 3,
                MaxWeightFired = 44.00,
                PowderFactor = 12.80,
                RockFactor = 9.60,
                RockSG = 22.50,
                ValidationState = ValidationState.Valid,
                DesignFragmentation = new DesignFragmentation()
                {
                    P10 = 1.0,
                    P20 = 2.0,
                    P30 = 3.0,
                    P40 = 4.0,
                    P50 = 5.0,
                    P60 = 6.0,
                    P70 = 7.0,
                    P80 = 8.0,
                    P90 = 9.0,
                },
                ActualFragmentation = new ActualFragmentation()
                {
                    P10 = 1.0,
                    P20 = 2.0,
                    P30 = 3.0,
                    P40 = 4.0,
                    P50 = 5.0,
                    P60 = 6.0,
                    P70 = 7.0,
                    P80 = 8.0,
                    P90 = 9.0,
                    TopSize = 225.00,
                },
                DesignBoundary = new List<PolyGeom>()
                {
                    new PolyGeom()
                    {
                        PolyGeometry = new List<GPSCoordinate>()
                        {
                            new GPSCoordinate()
                            {
                                Elevation = 1,
                                Latitude = 2,
                                Longitude = 3,
                            },
                            new GPSCoordinate()
                            {
                                Elevation = 4,
                                Latitude = 5,
                                Longitude = 6
                            },
                            new GPSCoordinate()
                            {
                                Elevation = 7,
                                Latitude = 8,
                                Longitude = 9,
                            }
                        },
                    },
                },
                ActualBoundary = new List<PolyGeom>()
                {
                    new PolyGeom()
                    {
                        PolyGeometry = new List<GPSCoordinate>()
                        {
                            new GPSCoordinate()
                            {
                                Elevation = 1,
                                Latitude = 2,
                                Longitude = 3,
                            },
                            new GPSCoordinate()
                            {
                                Elevation = 4,
                                Latitude = 5,
                                Longitude = 6
                            },
                            new GPSCoordinate()
                            {
                                Elevation = 7,
                                Latitude = 8,
                                Longitude = 9,
                            }
                        },
                    },
                },
                Holes = null,
            };

            return request;
        }
        #endregion
    }
}
