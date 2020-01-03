using System;

using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class NameBasedPropertyValidationTest
    {
        private NamedBaseProperty _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new NamedBaseProperty()
            {
                BaseProperties = new BaseProperty()
                {
                    CreatedAt = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    UpdatedAt = DateTime.UtcNow,
                },
                Name = "Foo",
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
        public void BasePropertyTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // NOT null
            _request.BaseProperties = null; 
            Assert.IsFalse(_validator.Validate(_request, out errors));
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void NameTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Name = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
            // NOT null, empty, whitespace
            _request.Name = null;
            Assert.IsFalse(_validator.Validate(_request, out errors));
            _request.Name = "";
            Assert.IsFalse(_validator.Validate(_request, out errors));
            _request.Name = "\t\r\n";
            Assert.IsFalse(_validator.Validate(_request, out errors));
        }
    }
}
