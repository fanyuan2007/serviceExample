using System.Collections.Generic;

using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class PolyGeomValidationTest
    {
        private PolyGeom _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new PolyGeom()
            {
                PolyGeometry = new List<GPSCoordinate>()
                {
                    new GPSCoordinate()
                    {
                        Latitude = 1,
                        Longitude = 2,
                        Elevation = 3,
                    },
                    new GPSCoordinate()
                    {
                        Latitude = 4,
                        Longitude = 5,
                        Elevation = 6,
                    },
                    new GPSCoordinate()
                    {
                        Latitude = 7,
                        Longitude = 8,
                        Elevation = 9,
                    }
                }
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
        public void PolyGeometryTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Must not be null.
            _request.PolyGeometry = null;
            Assert.IsFalse(_validator.Validate(_request, out errors));

            // Must contain at least two items.
            var coords = new List<GPSCoordinate>();
            _request.PolyGeometry = coords;
            Assert.IsFalse(_validator.Validate(_request, out errors));

            coords.Add(new GPSCoordinate());
            Assert.IsFalse(_validator.Validate(_request, out errors));

            coords.Add(new GPSCoordinate());
            Assert.IsTrue(_validator.Validate(_request, out errors));
        }
    }
}
