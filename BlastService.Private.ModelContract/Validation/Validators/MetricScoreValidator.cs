using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for MetricScore requests.
    /// </summary>
    public class MetricScoreValidator : ValidatorBase<MetricScore>
    {
        /// <summary>
        /// Instantiates a new MetricScore validator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public MetricScoreValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single MetricScore request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context is not a MetricScore request.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The MetricScore request to validate.</param>
        /// <returns>True if there were no errors found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }


            var ctx = context as MetricScore;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (String.IsNullOrWhiteSpace(ctx.Name))
            {
                var message = String.Format(_fmtNullEmptyWhitespace, nameof(ctx.Name));
                var error = new StringResult(nameof(ctx.Name), message);
                currentNode.AddResult(error);
            }

            if (Double.IsNaN(ctx.Weight) ||
                ctx.Weight <= 0)
            {
                var message = String.Format(_fmtDoubleGreaterThan, nameof(ctx.Weight), 0.0);
                var error = new StringResult(nameof(ctx.Weight), message);
                currentNode.AddResult(error);
            }

            if (Double.IsNaN(ctx.Score))
            {
                var message = String.Format(_fmtDoubleValid, nameof(ctx.Score));
                var error = new StringResult(nameof(ctx.Score), message);
                currentNode.AddResult(error);
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
