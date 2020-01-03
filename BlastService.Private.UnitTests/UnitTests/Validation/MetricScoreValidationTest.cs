using System;

using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class MetricScoreValidationTest
    {
        private MetricScore _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new MetricScore()
            {
                Name = "Foo",
                Score = 4.5,
                Weight = 2.25,
            };

            _validator = RequestValidatorFactory.CreateValidator();
        }
        [TearDown]
        public void TearDown()
        {
            _request = null;
            _validator = null;
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
        }

        // Double comparison
        [TestCase(Double.NaN, false)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(1.0, true)]
        public void WeightTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Weight = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(Double.NaN, false)]
        public void ScoreTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Score = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
    }
}
