using System;
using System.Collections.Generic;

using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class HoleValidationTest
    {
        private HoleRequest _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new HoleRequest()
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

        // Double comparison
        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(1.0, true)]
        public void AreaOfInfluenceTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.AreaOfInfluence = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(1.0, true)]
        public void VolumeOfInfluenceTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.VolumeOfInfluence = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void HoleStateTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.HoleState = value;

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

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void LayoutTypeTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.LayoutType = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
        
        // Double comparison
        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(1.0, true)]
        public void DesignDiameterTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.DesignDiameter = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, true)]
        [TestCase(Double.NaN, false)]
        public void DesignBenchCollarTest(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.DesignBenchCollar = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, true)]
        [TestCase(Double.NaN, false)]
        public void DesignBenchToeTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.DesignBenchToe = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(Double.NaN, false)]
        public void DesignSubDrillTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.DesignSubDrill = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(Double.NaN, false)]
        public void AccuracyTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Accuracy = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(Double.NaN, false)]
        public void LengthAccuracyTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Accuracy = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void ChargeTemplateNameTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.ChargeTemplateName = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(null, true)]
        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(1.0, true)]
        public void FragmentSizeTest(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.FragmentSize = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, true)]
        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(1.0, true)]
        public void PowderFactorTest(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.PowderFactor = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
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

        [Test]
        public void ActualFragmentationTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Null
            _request.ActualFragmentation = null; 
            Assert.IsTrue(_validator.Validate(_request, out errors));
        }

        [Test]
        public void DesignChargeProfileTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Null
            _request.DesignChargeProfile = null; 
            Assert.IsTrue(_validator.Validate(_request, out errors));

            // Empty list is NOT okay.
            _request.DesignChargeProfile = new List<ChargeInterval>();
            Assert.IsFalse(_validator.Validate(_request, out errors));
        }

        [Test]
        public void ActualChargeProfileTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Null
            _request.ActualChargeProfile = null; 
            Assert.IsTrue(_validator.Validate(_request, out errors));

            // Empty list is NOT okay.
            _request.ActualChargeProfile = new List<ChargeInterval>();
            Assert.IsFalse(_validator.Validate(_request, out errors));
        }

        [Test]
        public void DesignHoleTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Null
            _request.DesignHole = null; 
            Assert.IsFalse(_validator.Validate(_request, out errors));
        }

        [Test]
        public void ActualHoleTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Null
            _request.ActualHole = null; 
            Assert.IsTrue(_validator.Validate(_request, out errors));
        }
        
        [Test]
        public void DesignTraceTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Null
            _request.DesignTrace = null; 
            Assert.IsTrue(_validator.Validate(_request, out errors));

            // Must have at least two points.
            var coordList = new List<GPSCoordinate>();
            _request.DesignTrace = coordList;
            Assert.IsFalse(_validator.Validate(_request, out errors));
            
            coordList.Add(new GPSCoordinate() { Elevation = 0, Latitude = 0, Longitude = 0 });
            Assert.IsFalse(_validator.Validate(_request, out errors));

            coordList.Add(new GPSCoordinate() { Elevation = 0, Latitude = 0, Longitude = 0 });
            Assert.IsTrue(_validator.Validate(_request, out errors));
        }

        [Test]
        public void ActualTraceTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Null
            _request.ActualTrace = null; 
            Assert.IsTrue(_validator.Validate(_request, out errors));

            // Must have at least two points.
            var coordList = new List<GPSCoordinate>();
            _request.ActualTrace = coordList;
            Assert.IsFalse(_validator.Validate(_request, out errors));

            coordList.Add(new GPSCoordinate() { Elevation = 0, Latitude = 0, Longitude = 0 });
            Assert.IsFalse(_validator.Validate(_request, out errors));

            coordList.Add(new GPSCoordinate() { Elevation = 0, Latitude = 0, Longitude = 0 });
            Assert.IsTrue(_validator.Validate(_request, out errors));
        }
    }
}
