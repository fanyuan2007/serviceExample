using System;

using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class HoleStructureValidationTest
    {
        private HoleStructure _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new HoleStructure()
            {
                Azimuth = 22.5,
                Dip = -45.0,
                Length = 15.0,
                // Yes, I know these coordinates don't make sense geometrically,
                // but we're not testing calculation.
                Collar = new GPSCoordinate()
                {
                    Elevation = 10.0,
                    Latitude = 15.25,
                    Longitude = 133.25,
                },
                Toe = new GPSCoordinate()
                {
                    Elevation = -15.0,
                    Latitude = 25.25,
                    Longitude = 133.25,
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

        // Double comparison
        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, true)]
        [TestCase(1.0, true)]
        [TestCase(359.0, true)]
        [TestCase(360.0, true)]
        [TestCase(361.0, false)]
        public void AzimuthTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Azimuth = value;

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
        public void DipTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Dip = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, true)]
        [TestCase(1.0, true)]
        public void LengthTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Length = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CollarTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // NOT Null
            _request.Collar = null;
            Assert.False(_validator.Validate(_request, out errors));
        }

        [Test]
        public void ToeTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // NOT Null
            _request.Toe = null;
            Assert.False(_validator.Validate(_request, out errors));
        }
    }
}
