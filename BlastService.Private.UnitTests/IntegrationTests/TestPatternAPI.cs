using BlastService.Private.ModelContract;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace BlastService.Private.Tests.IntegrationTests
{
    [TestFixture]
    public class TestPatternAPI
    {
        private TestHttpClient _client;
        private ProjectRequest _project;
        private PatternRequest _pattern;
        private string _baseUrl;

        [SetUp]
        public void Setup()
        {
            _client = new TestHttpClient();
            _project = new ProjectRequest()
            {
                // Add default project values.
                NameBasedProperties = new NamedBaseProperty()
                {
                    Name = "TestPatternProject",
                    BaseProperties = new BaseProperty()
                    {
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                    },
                },

                Description = "This project is for testing pattern purpose.",
                Unit = MeasurementUnit.Metric,
                TimeZone = new UTCTimeZone()
                {
                    OffsetHours = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).Hours,
                    OffsetMinutes = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).Minutes,
                    IdName = TimeZoneInfo.Local.Id
                },

                LocalTransformation = null,
                Mapping = null,
            };

            _pattern = CreatePatternRequest(PatternFields.All);
            _baseUrl = "api/v0.3";
            // Use HttpClient to add default project & pattern
            HttpResponseMessage httpRespM;
            _client.PostProjectTest(_baseUrl, _project, out httpRespM);
            Assert.IsTrue(httpRespM.IsSuccessStatusCode);
            httpRespM = _client.PostPatternTest(_baseUrl, _pattern, _project.NameBasedProperties.BaseProperties.Id);
            Assert.IsTrue(httpRespM.IsSuccessStatusCode);
        }

        [TearDown]
        public void TearDown()
        {
            // Remove project using HttpClient
            HttpResponseMessage httpRespM;
            _client.DeleteProjectById(_baseUrl, _project.NameBasedProperties.BaseProperties.Id, out httpRespM);
            Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.NoContent);
            _client.DisposeClient();
        }

        [Test]
        public void GetAllPattern_ExistingRoute_Test()
        {
            HttpResponseMessage httpRespM;
            var patternResponses = _client.GetAllPatternsTest(_baseUrl, _project.NameBasedProperties.BaseProperties.Id, out httpRespM);
            if(patternResponses != null)
            {
                Console.WriteLine("number of projects returned: {0}", patternResponses.Count);
            }
            // Return empty list should still be successed in http communication
            Assert.IsNotNull(patternResponses);
            Assert.IsTrue(patternResponses.Count > 0);
            Assert.IsTrue(httpRespM.IsSuccessStatusCode);
        }

        [Test]
        public void GetPatternById_Test()
        {
            HttpResponseMessage httpRespM;
            var patternResponse = _client.GetPatternTest(_baseUrl, 
                _project.NameBasedProperties.BaseProperties.Id, 
                _pattern.NameBasedProperties.BaseProperties.Id, out httpRespM);
            Console.WriteLine("The Status Code returned : {0}", httpRespM.StatusCode);
            Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(patternResponse);
        }

        [Test]
        public void GetPatternById_NonExistPattern_Test()
        {
            var newPattern = CreatePatternRequest(PatternFields.All);

            HttpResponseMessage httpRespM;
            var patternResponse = _client.GetPatternTest(_baseUrl,
                _project.NameBasedProperties.BaseProperties.Id,
                newPattern.NameBasedProperties.BaseProperties.Id, out httpRespM);
            Console.WriteLine("The Status Code returned : {0}", httpRespM.StatusCode);
            Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.NotFound);
            Assert.IsNull(patternResponse);
        }

        [TestCase(PatternFields.NameBasedProperties)]
        [TestCase(PatternFields.Stage)]
        [TestCase(PatternFields.FaceAngle)]
        [TestCase(PatternFields.SubDrill)]
        [TestCase(PatternFields.HoleUsage)]
        [TestCase(PatternFields.PatternType)]
        [TestCase(PatternFields.Purpose)]
        [TestCase(PatternFields.PowderFactor, true)]
        [TestCase(PatternFields.RockFactor, true)]
        [TestCase(PatternFields.RockSG, true)]
        [TestCase(PatternFields.ValidationState)]
        [TestCase(PatternFields.DesignBoundary)]
        public void PostPattern_OmitRequiredFields_Test(PatternFields field, bool rangeExcludesZero = false)
        {
            var patternOmitRequiredField = CreatePatternRequest(field);

            var fieldName = field.ToString();
            var info = typeof(PatternRequest).GetProperty(fieldName);
            bool isRealOrEnumType = info.PropertyType == typeof(double) ||
                                    info.PropertyType.IsEnum;

            var httpRespM = _client.PostPatternTest(_baseUrl, patternOmitRequiredField, _project.NameBasedProperties.BaseProperties.Id);
            Console.WriteLine("The Status Code returned : {0}", httpRespM.StatusCode);       

            // Real type and enum type will have default values thus it will not be omitted
            // They can, however, have restricted value ranges which exclude 0.
            if (rangeExcludesZero)
            {
                // Cannot have default, as their range excludes 0.
                Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.BadRequest);
            }
            else if(isRealOrEnumType)
            {
                Assert.IsTrue(httpRespM.IsSuccessStatusCode);
            }
            else
            {
                Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.BadRequest);
            }
        }

        [TestCase(PatternFields.Bench)]
        [TestCase(PatternFields.Pit)]
        [TestCase(PatternFields.Phase)]
        [TestCase(PatternFields.Area)]
        [TestCase(PatternFields.Description)]
        [TestCase(PatternFields.Volume)]
        [TestCase(PatternFields.HoleLength)]
        [TestCase(PatternFields.Scoring)]
        [TestCase(PatternFields.GeologyCode)]
        [TestCase(PatternFields.PatternTemplateName)]
        [TestCase(PatternFields.ChargingTemplateName)]
        [TestCase(PatternFields.IsElectronic)]
        [TestCase(PatternFields.MaxHoleFired)]
        [TestCase(PatternFields.MaxWeightFired)]
        [TestCase(PatternFields.DesignFragmentation)]
        [TestCase(PatternFields.ActualFragmentation)]
        [TestCase(PatternFields.ActualBoundary)]
        [TestCase(PatternFields.Holes)]
        public void PostPattern_OmitNonRequiredFields_Test(PatternFields field)
        {
            var patternOmitNonRequiredField = CreatePatternRequest(field);

            HttpResponseMessage httpRespM;
            httpRespM = _client.PostPatternTest(_baseUrl, patternOmitNonRequiredField, _project.NameBasedProperties.BaseProperties.Id);
            Console.WriteLine("The Status Code returned : {0}", httpRespM.StatusCode);
            Assert.IsTrue(httpRespM.IsSuccessStatusCode);

            var response = _client.GetPatternTest(_baseUrl, _project.NameBasedProperties.BaseProperties.Id,
                patternOmitNonRequiredField.NameBasedProperties.BaseProperties.Id, out httpRespM);
            Console.WriteLine("The Status Code returned for correct route: {0}", httpRespM.StatusCode);
            Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void DeletePattern_Test()
        {
            // Not implemented
        }

        [Test]
        public void PatternEnumTypeAttr_Test()
        {
            var patternAllFields = CreatePatternRequest(PatternFields.All);
            HttpResponseMessage httpRespM;
            httpRespM = _client.PostPatternTest(_baseUrl, patternAllFields, _project.NameBasedProperties.BaseProperties.Id);
            Console.WriteLine("The Status Code returned : {0}", httpRespM.StatusCode);
            Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.Created);

            var response = _client.GetPatternTest(_baseUrl, _project.NameBasedProperties.BaseProperties.Id,
                patternAllFields.NameBasedProperties.BaseProperties.Id, out httpRespM);
            Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(Enum.IsDefined(typeof(PatternType), response.PatternType));
            Assert.IsTrue(Enum.IsDefined(typeof(ValidationState), response.ValidationState));
            if(response.Holes != null)
            {
                int count = 0;
                Assert.IsTrue(response.Holes.Count > 0);
                foreach(var hole in response.Holes)
                {
                    Console.WriteLine("Hole {0} validation state enum check", count);
                    Assert.IsTrue(Enum.IsDefined(typeof(ValidationState), hole.ValidationState));
                    count++;
                }
            }
        }

        [TestCase(3, 3)]
        [TestCase(3, 2)]
        [TestCase(3, 1)]
        [TestCase(3, 0)]
        [TestCase(2, 3)]
        [TestCase(2, 2)]
        [TestCase(2, 1)]
        [TestCase(2, 0)]
        [TestCase(1, 3)]
        [TestCase(1, 2)]
        [TestCase(1, 1)]
        [TestCase(1, 0)]
        public void PostPattern_VariousBoundaryPoints_Test(int designBoundaryPts, int actualBoundaryPts)
        {
            var patternSpecialBoundaryField = CreatePatternRequest_SpecialBoundary(PatternFields.All, designBoundaryPts, actualBoundaryPts);

            HttpResponseMessage httpRespM;
            httpRespM = _client.PostPatternTest(_baseUrl, patternSpecialBoundaryField, _project.NameBasedProperties.BaseProperties.Id);
            Console.WriteLine("The Status Code returned : {0}", httpRespM.StatusCode);

            if (designBoundaryPts < 2 || actualBoundaryPts < 2)
            {
                Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.BadRequest);
            }
            else
            {
                Assert.IsTrue(httpRespM.IsSuccessStatusCode);

                var response = _client.GetPatternTest(_baseUrl, _project.NameBasedProperties.BaseProperties.Id,
                patternSpecialBoundaryField.NameBasedProperties.BaseProperties.Id, out httpRespM);
                Console.WriteLine("The Status Code returned for correct route: {0}", httpRespM.StatusCode);
                Assert.IsTrue(httpRespM.IsSuccessStatusCode);
                Assert.IsNotNull(response);
            }
        }
        
        [TestCase(PatternFields.NameBasedProperties)]
        [TestCase(PatternFields.Stage)]
        [TestCase(PatternFields.FaceAngle)]
        [TestCase(PatternFields.SubDrill)]
        [TestCase(PatternFields.HoleUsage)]
        [TestCase(PatternFields.PatternType)]
        [TestCase(PatternFields.Purpose)]
        [TestCase(PatternFields.PowderFactor)]
        [TestCase(PatternFields.RockFactor)]
        [TestCase(PatternFields.RockSG)]
        [TestCase(PatternFields.ValidationState)]
        [TestCase(PatternFields.DesignBoundary)]
        public void PostPattern_RequiredFields_Nullable_Test(PatternFields field)
        {
            var patternField_Nullable = CreatePatternRequest(PatternFields.All);

            var fieldName = field.ToString();
            var info = typeof(PatternRequest).GetProperty(fieldName);
            bool isRealTypeOrEnumType = info.PropertyType == typeof(double) || 
                                        info.PropertyType.IsEnum;

            if (!isRealTypeOrEnumType)
            {
                try
                {
                    info.SetValue(patternField_Nullable, null);
                }
                catch
                {
                    throw;
                }
            }

            var httpRespM = _client.PostPatternTest(_baseUrl, patternField_Nullable, _project.NameBasedProperties.BaseProperties.Id);
            Console.WriteLine("The Status Code returned : {0}", httpRespM.StatusCode);
            // Required Fields as Real or Enum type cannot be set to null
            if (isRealTypeOrEnumType)
            {
                Assert.IsTrue(httpRespM.IsSuccessStatusCode);
            }
            else
            {
                Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.BadRequest);
            }
        }

        [TestCase(PatternFields.Description)]
        [TestCase(PatternFields.Bench)]
        [TestCase(PatternFields.Pit)]
        [TestCase(PatternFields.Phase)]
        [TestCase(PatternFields.Area)]
        [TestCase(PatternFields.Volume)]
        [TestCase(PatternFields.HoleLength)]
        [TestCase(PatternFields.Scoring)]
        [TestCase(PatternFields.GeologyCode)]
        [TestCase(PatternFields.PatternTemplateName)]
        [TestCase(PatternFields.ChargingTemplateName)]
        [TestCase(PatternFields.IsElectronic)]
        [TestCase(PatternFields.MaxHoleFired)]
        [TestCase(PatternFields.MaxWeightFired)]
        [TestCase(PatternFields.DesignFragmentation)]
        [TestCase(PatternFields.ActualFragmentation)]
        [TestCase(PatternFields.ActualBoundary)]
        [TestCase(PatternFields.Holes)]
        public void PostPattern_NonRequiredFields_Nullable_Test(PatternFields field)
        {
            var patternField_Nullable = CreatePatternRequest(PatternFields.All);

            var fieldName = field.ToString();
            var info = typeof(PatternRequest).GetProperty(fieldName);

            try
            {
                info.SetValue(patternField_Nullable, null);
            }
            catch
            {
                throw;
            }

            var httpRespM = _client.PostPatternTest(_baseUrl, patternField_Nullable, _project.NameBasedProperties.BaseProperties.Id);
            Console.WriteLine("The Status Code returned : {0}", httpRespM.StatusCode);
            Assert.IsTrue(httpRespM.IsSuccessStatusCode);

            var response = _client.GetPatternTest(_baseUrl, _project.NameBasedProperties.BaseProperties.Id,
                patternField_Nullable.NameBasedProperties.BaseProperties.Id, out httpRespM);
            Console.WriteLine("The Status Code returned for correct route: {0}", httpRespM.StatusCode);
            Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.OK);
        }

        private PatternRequest CreatePatternRequest(PatternFields omitField)
        {
            var patternRequest = new PatternRequest();

            var gpsLists = new List<GPSCoordinate>();
            GPSCoordinate gps1 = new GPSCoordinate()
            {
                Latitude = 20.0,
                Longitude = 20.0,
                Elevation = 0.0
            };
            GPSCoordinate gps2 = new GPSCoordinate()
            {
                Latitude = 20.0,
                Longitude = 20.0,
                Elevation = 10.0
            };
            GPSCoordinate gps3 = new GPSCoordinate()
            {
                Latitude = 20.0,
                Longitude = 20.0,
                Elevation = 20.0
            };
            gpsLists.Add(gps1);
            gpsLists.Add(gps2);
            gpsLists.Add(gps3);

            if (omitField != PatternFields.NameBasedProperties)
            {
                patternRequest.NameBasedProperties = new NamedBaseProperty()
                {
                    Name = "PatternWithoutGeologyCode",
                    BaseProperties = new BaseProperty()
                    {
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                    },
                };
            }

            if (omitField != PatternFields.Stage)
            {
                patternRequest.Stage = "testStage";
            }

            if (omitField != PatternFields.Description)
            {
                patternRequest.Description = "This is the pattern without geology code.";
            }

            if (omitField != PatternFields.FaceAngle)
            {
                patternRequest.FaceAngle = 15.0;
            }

            if (omitField != PatternFields.SubDrill)
            {
                patternRequest.SubDrill = 10.0;
            }

            if (omitField != PatternFields.Bench)
            {
                patternRequest.Bench = "test bench";
            }

            if (omitField != PatternFields.Pit)
            {
                patternRequest.Pit = "test pit";
            }

            if (omitField != PatternFields.Phase)
            {
                patternRequest.Phase = "testing phase";
            }

            if (omitField != PatternFields.HoleUsage)
            {
                patternRequest.HoleUsage = "test hole usage";
            }

            if (omitField != PatternFields.PatternType)
            {
                patternRequest.PatternType = PatternType.Blast;
            }

            if (omitField != PatternFields.Purpose)
            {
                patternRequest.Purpose = "testing purpose";
            }

            if (omitField != PatternFields.IsElectronic)
            {
                patternRequest.IsElectronic = true;
            }

            if (omitField != PatternFields.PowderFactor)
            {
                patternRequest.PowderFactor = 0.8;
            }

            if (omitField != PatternFields.RockFactor)
            {
                patternRequest.RockFactor = 0.9;
            }

            if (omitField != PatternFields.RockSG)
            {
                patternRequest.RockSG = 2.0;
            }

            if (omitField != PatternFields.ValidationState)
            {
                patternRequest.ValidationState = ValidationState.Valid;
            }

            if (omitField != PatternFields.DesignBoundary)
            {
                var bound = new List<PolyGeom>();
                PolyGeom geo = new PolyGeom()
                {
                    PolyGeometry = gpsLists
                };
                bound.Add(geo);
                patternRequest.DesignBoundary = bound;
            }

            if (omitField != PatternFields.Area)
            {
                patternRequest.Area = 100.0;
            }

            if (omitField != PatternFields.Volume)
            {
                patternRequest.Volume = 1000.0;
            }

            if (omitField != PatternFields.HoleLength)
            {
                patternRequest.HoleLength = new HoleLength()
                {
                    Total = 10.0,
                    Average = 2.0,
                };
            }

            if (omitField != PatternFields.Scoring)
            {
                var metricScore = new List<MetricScore>();
                MetricScore scor = new MetricScore()
                {
                    Name = "score",
                    Weight = 1.0,
                    Score = 10.0
                };
                metricScore.Add(scor);
                patternRequest.Scoring = new Scoring()
                {
                    TotalScore = 10.0,
                    MetricScore = metricScore
                };
            }

            if (omitField != PatternFields.PatternTemplateName)
            {
                patternRequest.PatternTemplateName = "testTemp";
            }

            if (omitField != PatternFields.GeologyCode)
            {
                patternRequest.GeologyCode = "test code";
            }

            if (omitField != PatternFields.ChargingTemplateName)
            {
                patternRequest.ChargingTemplateName = "testCharge";
            }

            if (omitField != PatternFields.MaxHoleFired)
            {
                patternRequest.MaxHoleFired = 20;
            }

            if (omitField != PatternFields.MaxWeightFired)
            {
                patternRequest.MaxWeightFired = 1.0;
            }

            if (omitField != PatternFields.DesignFragmentation)
            {
                patternRequest.DesignFragmentation = new DesignFragmentation()
                {
                    P10 = 0.1,
                    P20 = 0.2,
                    P30 = 0.3,
                    P40 = 0.4,
                    P50 = 0.5,
                    P60 = 0.6,
                    P70 = 0.7,
                    P80 = 0.8,
                    P90 = 0.9,
                };
            }

            if (omitField != PatternFields.ActualFragmentation)
            {
                patternRequest.ActualFragmentation = new ActualFragmentation()
                {
                    P10 = 0.1,
                    P20 = 0.2,
                    P30 = 0.3,
                    P40 = 0.4,
                    P50 = 0.5,
                    P60 = 0.6,
                    P70 = 0.7,
                    P80 = 0.8,
                    P90 = 0.9,
                    TopSize = 1.0,
                };
            }

            if (omitField != PatternFields.ActualBoundary)
            {
                var bound = new List<PolyGeom>();
                PolyGeom geo = new PolyGeom()
                {
                    PolyGeometry = gpsLists
                };
                bound.Add(geo);
                patternRequest.ActualBoundary = bound;
            }

            if (omitField != PatternFields.Holes)
            {
                var holes = new List<HoleRequest>();
                var chargeIntervals = new List<ChargeInterval>();
                ChargeInterval ci_1 = new ChargeInterval()
                {
                    From = 1.0,
                    To = 2.0,
                    Amount = 10,
                    Consumable = "abc",
                    Deck = "deck1"
                };
                chargeIntervals.Add(ci_1);

                HoleRequest hole1 = new HoleRequest()
                {
                    // Required
                    NameBasedProperties = new NamedBaseProperty
                    {
                        Name = "TestHole1",
                        BaseProperties = new BaseProperty()
                        {
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            Id = Guid.NewGuid(),
                        },
                    },
                    DrillPatternId = Guid.NewGuid(),
                    AreaOfInfluence = 100.0,
                    VolumeOfInfluence = 1000.0,
                    HoleState = "testHole1_state",
                    HoleUsage = "hole1_usage",
                    LayoutType = "hole1_layout",
                    DesignDiameter = 20.0,
                    DesignSubDrill = 100.0,
                    ValidationState = ValidationState.Invalid,
                    DesignHole = new HoleStructure()
                    {
                        Azimuth = 90,
                        Dip = 90,
                        Length = 10,
                        Collar = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        },
                        Toe = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 20.0,
                            Elevation = 20.0
                        }
                    },

                    // Optional
                    BlastPatternId = Guid.NewGuid(),
                    DesignBenchCollar = 10.0,
                    DesignBenchToe = 11.0,
                    Accuracy = 0.01,
                    LengthAccuracy = 0.01,
                    DesignChargeInfo = new ChargeInfo()
                    {
                        Weight = 1.0,
                        Thickness = 5.0,
                    },
                    ActualChargeInfo = new ChargeInfo()
                    {
                        Weight = 1.0,
                        Thickness = 5.0,
                    },
                    ChargeTemplateName = "hole1_template",
                    FragmentSize = 1.0,
                    PowderFactor = 0.5,
                    GeologyCode = "hole1_code",
                    ActualFragmentation = new ActualFragmentation()
                    {
                        P10 = 0.11,
                        P20 = 0.21,
                        P30 = 0.31,
                        P40 = 0.41,
                        P50 = 0.51,
                        P60 = 0.61,
                        P70 = 0.71,
                        P80 = 0.81,
                        P90 = 0.91,
                        TopSize = 1.0,
                    },

                    DesignChargeProfile = chargeIntervals,
                    ActualChargeProfile = chargeIntervals,
                    ActualHole = new HoleStructure()
                    {
                        Azimuth = 90,
                        Dip = 90,
                        Length = 10,
                        Collar = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        },
                        Toe = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        }
                    },
                    DesignTrace = gpsLists,
                    ActualTrace = gpsLists,
                };
                HoleRequest hole2 = new HoleRequest()
                {
                    // Required
                    NameBasedProperties = new NamedBaseProperty
                    {
                        Name = "TestHole2",
                        BaseProperties = new BaseProperty()
                        {
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            Id = Guid.NewGuid(),
                        },
                    },
                    DrillPatternId = Guid.NewGuid(),
                    AreaOfInfluence = 100.0,
                    VolumeOfInfluence = 1000.0,
                    HoleState = "testHole2_state",
                    HoleUsage = "hole2_usage",
                    LayoutType = "hole2_layout",
                    DesignDiameter = 20.0,
                    DesignSubDrill = 100.0,
                    ValidationState = ValidationState.Invalid,
                    DesignHole = new HoleStructure()
                    {
                        Azimuth = 90,
                        Dip = 90,
                        Length = 10,
                        Collar = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        },
                        Toe = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        }
                    },

                    // Optional
                    BlastPatternId = Guid.NewGuid(),
                    DesignBenchCollar = 10.0,
                    DesignBenchToe = 11.0,
                    Accuracy = 0.01,
                    LengthAccuracy = 0.01,
                    DesignChargeInfo = new ChargeInfo()
                    {
                        Weight = 1.0,
                        Thickness = 5.0,
                    },
                    ActualChargeInfo = new ChargeInfo()
                    {
                        Weight = 1.0,
                        Thickness = 5.0,
                    },
                    ChargeTemplateName = "hole2_template",
                    FragmentSize = 1.0,
                    PowderFactor = 0.5,
                    GeologyCode = "hole2_code",
                    ActualFragmentation = new ActualFragmentation()
                    {
                        P10 = 0.11,
                        P20 = 0.21,
                        P30 = 0.31,
                        P40 = 0.41,
                        P50 = 0.51,
                        P60 = 0.61,
                        P70 = 0.71,
                        P80 = 0.81,
                        P90 = 0.91,
                        TopSize = 1.0,
                    },
                    DesignChargeProfile = chargeIntervals,
                    ActualChargeProfile = chargeIntervals,
                    ActualHole = new HoleStructure()
                    {
                        Azimuth = 90,
                        Dip = 90,
                        Length = 10,
                        Collar = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        },
                        Toe = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        }
                    },
                    DesignTrace = gpsLists,
                    ActualTrace = gpsLists,
                };
                holes.Add(hole1);
                holes.Add(hole2);

                patternRequest.Holes = holes;
            }

            return patternRequest;
        }

        private PatternRequest CreatePatternRequest_SpecialBoundary(PatternFields omitField, int dPtNum, int aPtNum)
        {
            var patternRequest = new PatternRequest();

            var gpsLists = new List<GPSCoordinate>();
            var num = Math.Max(Math.Max(dPtNum, 3), aPtNum);

            for(int i = 0; i<num; ++i)
            {
                var gpsPt = new GPSCoordinate()
                {
                    Latitude = 20.0*i,
                    Longitude = 20.0*(i+1),
                    Elevation = 50
                };
                gpsLists.Add(gpsPt);
            }

            if (omitField != PatternFields.NameBasedProperties)
            {
                patternRequest.NameBasedProperties = new NamedBaseProperty()
                {
                    Name = "PatternWithoutGeologyCode",
                    BaseProperties = new BaseProperty()
                    {
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                    },
                };
            }

            if (omitField != PatternFields.Stage)
            {
                patternRequest.Stage = "testStage";
            }

            if (omitField != PatternFields.Description)
            {
                patternRequest.Description = "This is the pattern without geology code.";
            }

            if (omitField != PatternFields.FaceAngle)
            {
                patternRequest.FaceAngle = 20.0;
            }

            if (omitField != PatternFields.SubDrill)
            {
                patternRequest.SubDrill = 10.0;
            }

            if (omitField != PatternFields.Bench)
            {
                patternRequest.Bench = "test bench";
            }

            if (omitField != PatternFields.Pit)
            {
                patternRequest.Pit = "test pit";
            }

            if (omitField != PatternFields.Phase)
            {
                patternRequest.Phase = "testing phase";
            }

            if (omitField != PatternFields.HoleUsage)
            {
                patternRequest.HoleUsage = "test hole usage";
            }

            if (omitField != PatternFields.PatternType)
            {
                patternRequest.PatternType = PatternType.Blast;
            }

            if (omitField != PatternFields.Purpose)
            {
                patternRequest.Purpose = "testing purpose";
            }

            if (omitField != PatternFields.IsElectronic)
            {
                patternRequest.IsElectronic = true;
            }

            if (omitField != PatternFields.PowderFactor)
            {
                patternRequest.PowderFactor = 0.8;
            }

            if (omitField != PatternFields.RockFactor)
            {
                patternRequest.RockFactor = 0.9;
            }

            if (omitField != PatternFields.RockSG)
            {
                patternRequest.RockSG = 2.0;
            }

            if (omitField != PatternFields.ValidationState)
            {
                patternRequest.ValidationState = ValidationState.Valid;
            }

            if (omitField != PatternFields.DesignBoundary)
            {
                var bound = new List<PolyGeom>();
                var newGpsList = new List<GPSCoordinate>();
                for(int i = 0; i< dPtNum; ++i)
                {
                    if (i < gpsLists.Count)
                    {
                        newGpsList.Add(gpsLists[i]);
                    }
                }

                PolyGeom geo = new PolyGeom()
                {
                    PolyGeometry = newGpsList
                };
                bound.Add(geo);
                patternRequest.DesignBoundary = bound;
            }

            if (omitField != PatternFields.Area)
            {
                patternRequest.Area = 100.0;
            }

            if (omitField != PatternFields.Volume)
            {
                patternRequest.Volume = 1000.0;
            }

            if (omitField != PatternFields.HoleLength)
            {
                patternRequest.HoleLength = new HoleLength()
                {
                    Total = 10.0,
                    Average = 2.0,
                };
            }

            if (omitField != PatternFields.Scoring)
            {
                var metricScore = new List<MetricScore>();
                MetricScore scor = new MetricScore()
                {
                    Name = "score",
                    Weight = 1.0,
                    Score = 10.0
                };
                metricScore.Add(scor);
                patternRequest.Scoring = new Scoring()
                {
                    TotalScore = 10.0,
                    MetricScore = metricScore
                };
            }

            if (omitField != PatternFields.PatternTemplateName)
            {
                patternRequest.PatternTemplateName = "testTemp";
            }

            if (omitField != PatternFields.GeologyCode)
            {
                patternRequest.GeologyCode = "test code";
            }

            if (omitField != PatternFields.ChargingTemplateName)
            {
                patternRequest.ChargingTemplateName = "testCharge";
            }

            if (omitField != PatternFields.MaxHoleFired)
            {
                patternRequest.MaxHoleFired = 20;
            }

            if (omitField != PatternFields.MaxWeightFired)
            {
                patternRequest.MaxWeightFired = 1.0;
            }

            if (omitField != PatternFields.DesignFragmentation)
            {
                patternRequest.DesignFragmentation = new DesignFragmentation()
                {
                    P10 = 0.1,
                    P20 = 0.2,
                    P30 = 0.3,
                    P40 = 0.4,
                    P50 = 0.5,
                    P60 = 0.6,
                    P70 = 0.7,
                    P80 = 0.8,
                    P90 = 0.9,
                };
            }

            if (omitField != PatternFields.ActualFragmentation)
            {
                patternRequest.ActualFragmentation = new ActualFragmentation()
                {
                    P10 = 0.1,
                    P20 = 0.2,
                    P30 = 0.3,
                    P40 = 0.4,
                    P50 = 0.5,
                    P60 = 0.6,
                    P70 = 0.7,
                    P80 = 0.8,
                    P90 = 0.9,
                    TopSize = 20.0,
                };
            }

            if (omitField != PatternFields.ActualBoundary)
            {
                var bound = new List<PolyGeom>();
                var newGpsList = new List<GPSCoordinate>();
                for (int i = 0; i < aPtNum; ++i)
                {
                    if (i < gpsLists.Count)
                    {
                        newGpsList.Add(gpsLists[i]);
                    }
                }

                PolyGeom geo = new PolyGeom()
                {
                    PolyGeometry = newGpsList
                };
                bound.Add(geo);
                patternRequest.ActualBoundary = bound;
            }

            if (omitField != PatternFields.Holes)
            {
                var holes = new List<HoleRequest>();
                var chargeIntervals = new List<ChargeInterval>();
                ChargeInterval ci_1 = new ChargeInterval()
                {
                    From = 1.0,
                    To = 2.0,
                    Amount = 10,
                    Consumable = "abc",
                    Deck = "deck1"
                };
                chargeIntervals.Add(ci_1);

                HoleRequest hole1 = new HoleRequest()
                {
                    // Required
                    NameBasedProperties = new NamedBaseProperty
                    {
                        Name = "TestHole1",
                        BaseProperties = new BaseProperty()
                        {
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            Id = Guid.NewGuid(),
                        },
                    },
                    DrillPatternId = Guid.NewGuid(),
                    AreaOfInfluence = 100.0,
                    VolumeOfInfluence = 1000.0,
                    HoleState = "testHole1_state",
                    HoleUsage = "hole1_usage",
                    LayoutType = "hole1_layout",
                    DesignDiameter = 20.0,
                    DesignSubDrill = 100.0,
                    ValidationState = ValidationState.Invalid,
                    DesignHole = new HoleStructure()
                    {
                        Azimuth = 90,
                        Dip = 90,
                        Length = 10,
                        Collar = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        },
                        Toe = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        }
                    },

                    // Optional
                    BlastPatternId = Guid.NewGuid(),
                    DesignBenchCollar = 10.0,
                    DesignBenchToe = 11.0,
                    Accuracy = 0.01,
                    LengthAccuracy = 0.01,
                    DesignChargeInfo = new ChargeInfo()
                    {
                        Weight = 1.0,
                        Thickness = 5.0,
                    },
                    ActualChargeInfo = new ChargeInfo()
                    {
                        Weight = 1.0,
                        Thickness = 5.0,
                    },
                    ChargeTemplateName = "hole1_template",
                    FragmentSize = 1.0,
                    PowderFactor = 0.5,
                    GeologyCode = "hole1_code",
                    ActualFragmentation = new ActualFragmentation()
                    {
                        P10 = 0.11,
                        P20 = 0.21,
                        P30 = 0.31,
                        P40 = 0.41,
                        P50 = 0.51,
                        P60 = 0.61,
                        P70 = 0.71,
                        P80 = 0.81,
                        P90 = 0.91,
                    },

                    DesignChargeProfile = chargeIntervals,
                    ActualChargeProfile = chargeIntervals,
                    ActualHole = new HoleStructure()
                    {
                        Azimuth = 90,
                        Dip = 90,
                        Length = 10,
                        Collar = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        },
                        Toe = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        }
                    },
                    DesignTrace = gpsLists,
                    ActualTrace = gpsLists,
                };
                HoleRequest hole2 = new HoleRequest()
                {
                    // Required
                    NameBasedProperties = new NamedBaseProperty
                    {
                        Name = "TestHole2",
                        BaseProperties = new BaseProperty()
                        {
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            Id = Guid.NewGuid(),
                        },
                    },
                    DrillPatternId = Guid.NewGuid(),
                    AreaOfInfluence = 100.0,
                    VolumeOfInfluence = 1000.0,
                    HoleState = "testHole2_state",
                    HoleUsage = "hole2_usage",
                    LayoutType = "hole2_layout",
                    DesignDiameter = 20.0,
                    DesignSubDrill = 100.0,
                    ValidationState = ValidationState.Invalid,
                    DesignHole = new HoleStructure()
                    {
                        Azimuth = 90,
                        Dip = 90,
                        Length = 10,
                        Collar = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        },
                        Toe = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        }
                    },

                    // Optional
                    BlastPatternId = Guid.NewGuid(),
                    DesignBenchCollar = 10.0,
                    DesignBenchToe = 11.0,
                    Accuracy = 0.01,
                    LengthAccuracy = 0.01,
                    DesignChargeInfo = new ChargeInfo()
                    {
                        Weight = 1.0,
                        Thickness = 5.0,
                    },
                    ActualChargeInfo = new ChargeInfo()
                    {
                        Weight = 1.0,
                        Thickness = 5.0,
                    },
                    ChargeTemplateName = "hole2_template",
                    FragmentSize = 1.0,
                    PowderFactor = 0.5,
                    GeologyCode = "hole2_code",
                    ActualFragmentation = new ActualFragmentation()
                    {
                        P10 = 0.11,
                        P20 = 0.21,
                        P30 = 0.31,
                        P40 = 0.41,
                        P50 = 0.51,
                        P60 = 0.61,
                        P70 = 0.71,
                        P80 = 0.81,
                        P90 = 0.91,
                    },
                    DesignChargeProfile = chargeIntervals,
                    ActualChargeProfile = chargeIntervals,
                    ActualHole = new HoleStructure()
                    {
                        Azimuth = 90,
                        Dip = 90,
                        Length = 10,
                        Collar = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        },
                        Toe = new GPSCoordinate()
                        {
                            Latitude = 10.0,
                            Longitude = 10.0,
                            Elevation = 20.0
                        }
                    },
                    DesignTrace = gpsLists,
                    ActualTrace = gpsLists,
                };
                holes.Add(hole1);
                holes.Add(hole2);

                patternRequest.Holes = holes;
            }

            return patternRequest;
        }

    }
}