using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

using NUnit.Framework;

using System;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class BasePropertyValidationTest
    {
        private BaseProperty _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new BaseProperty()
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0),
                Id = Guid.NewGuid(),
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
        public void CreatedAtEarlierThanEqualToUpdatedAtTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // CreatedAt <= UpdatedAt
            _request.CreatedAt = _request.UpdatedAt;
            Assert.IsTrue(_validator.Validate(_request, out errors)); 
            _request.CreatedAt = _request.UpdatedAt.Add(new TimeSpan(1, 0, 0, 0));
            Assert.IsFalse(_validator.Validate(_request, out errors));
        }
    }
}
