using System;

using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class ProjectValidationTest
    {
        private ProjectRequest _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new ProjectRequest()
            {
                NameBasedProperties = new NamedBaseProperty()
                {
                    BaseProperties = new BaseProperty()
                    {
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                    },
                    Name = "foo",
                },
                Description = "asdf",
                Unit = MeasurementUnit.Metric,
                TimeZone = new UTCTimeZone()
                {
                    IdName = "UTC +8",
                    IsDaylightSavingTime = false,
                    OffsetHours = 8,
                    OffsetMinutes = 0,
                }
            };

            _validator = RequestValidatorFactory.CreateValidator();
        }

        [TearDown]
        public void TearDown()
        {
            _validator = null;
            _request = null;
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
        [TestCase("", true)]
        [TestCase(" \t\r\n", true)]
        public void DescriptionTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Description = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TimeZoneTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // NOT null
            _request.TimeZone = null;
            Assert.IsFalse(_validator.Validate(_request, out errors));
        }

        [Test]
        public void LocalTransformationTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            
            // Null
            _request.LocalTransformation = null;
            Assert.IsTrue(_validator.Validate(_request, out errors)); 
        }

        [Test]
        public void MappingTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Null
            _request.Mapping = null;
            Assert.IsTrue(_validator.Validate(_request, out errors));
        }
    }
}
