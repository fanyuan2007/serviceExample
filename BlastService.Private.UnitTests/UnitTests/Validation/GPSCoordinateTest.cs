using System;

using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class GPSCoordinateTest
    {
        private GPSCoordinate _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new GPSCoordinate()
            {
                Latitude = -31.9505,
                Longitude = 115.8605,
                Elevation = 55.25
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
        [TestCase(-91.0, false)]
        [TestCase(-90.0, false)]
        [TestCase(-89.0, true)]
        [TestCase(89.0, true)]
        [TestCase(90.0, false)]
        [TestCase(91.0, false)]
        public void LatitudeTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Latitude = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(Double.NaN, false)]
        [TestCase(-181.0, false)]
        [TestCase(-180.0, false)]
        [TestCase(-179.0, true)]
        [TestCase(179.0, true)]
        [TestCase(180.0, false)]
        [TestCase(181.0, false)]
        public void LongitudeTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Longitude = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(Double.NaN, false)]
        public void ElevationTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Longitude = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
    }
}
