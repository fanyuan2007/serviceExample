using System;
using System.Collections.Generic;

using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class ScoringValidationTest
    {
        private Scoring _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new Scoring()
            {
                TotalScore = 15.00,
                MetricScore = new List<MetricScore>()
                { 
                    new MetricScore()
                    {
                        Name = "Test",
                        Score = 15,
                        Weight = 1,
                    },
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
        public void TotalScoreTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // TotalScore != NaN
            _request.TotalScore = Double.NaN;
            Assert.IsFalse(_validator.Validate(_request, out errors)); 
        }

        [Test]
        public void MetricScoreTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Cannot be null or empty.
            _request.MetricScore = null;
            Assert.IsFalse(_validator.Validate(_request, out errors));

            _request.MetricScore = new List<MetricScore>();
            Assert.IsFalse(_validator.Validate(_request, out errors));
        }
    }
}
