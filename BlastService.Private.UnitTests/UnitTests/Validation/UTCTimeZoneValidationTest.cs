using System;

using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class UTCTimeZoneValidationTest
    {
        private UTCTimeZone _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new UTCTimeZone()
            {
                IdName = TimeZoneInfo.Local.Id,
                IsDaylightSavingTime = false,
                OffsetHours = 8,
                OffsetMinutes = 0,
            };

            _validator = RequestValidatorFactory.CreateValidator();
        }

        [TearDown]
        public void TearDown()
        {
            _request = null;
            _validator = null;
        }

        [TestCase(-13, false)]
        [TestCase(-12, true)]
        [TestCase(14, true)]
        [TestCase(15, false)]
        public void OffsetHoursTest(int value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.OffsetHours = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
        
        [TestCase(-60, false)]
        [TestCase(-59, true)]
        [TestCase(59, true)]
        [TestCase(60, false)]
        public void OffsetMinutesTest(int value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.OffsetMinutes = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void IdNameTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.IdName = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
    }
}
