using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for ChargeInterval requests.
    /// </summary>
    public class ChargeIntervalValidator : ValidatorBase<ChargeInterval>
    {
        /// <summary>
        /// Instantiates a new ChargeInterval validator with provided settings and
        /// the owning RequestValidator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public ChargeIntervalValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single ChargeInterval request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context object is not a ChargeInterval request.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The ChargeInterval request to validate.</param>
        /// <returns>True if there were no errors found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as ChargeInterval;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new NotImplementedException(_validatorBadErrorNode);
            }

            if (Double.IsNaN(ctx.From) || ctx.From < 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThanEqualTo, nameof(ctx.From), 0.0);
                var error = new StringResult(nameof(ctx.From), message);
                currentNode.AddResult(error);
            }
            if (Double.IsNaN(ctx.To) || ctx.To < 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThanEqualTo, nameof(ctx.To), 0.0);
                var error = new StringResult(nameof(ctx.To), message);
                currentNode.AddResult(error);
            }
            if (!Double.IsNaN(ctx.To) && !Double.IsNaN(ctx.From) &&
                ctx.From > ctx.To)
            {
                var message = String.Format(_fmtDoubleGreaterThanEqualTo, nameof(ctx.To), nameof(ctx.From));
                var error = new StringResult(nameof(ctx.From), message);
                currentNode.AddResult(error);
            }
            if (String.IsNullOrWhiteSpace(ctx.Consumable))
            {
                var message = String.Format(_fmtNullEmptyWhitespace, nameof(ctx.Consumable));
                var error = new StringResult(nameof(ctx.Consumable), message);
                currentNode.AddResult(error);
            }
            if (Double.IsNaN(ctx.Amount) || ctx.Amount < 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThanEqualTo, nameof(ctx.Amount), 0.0);
                var error = new StringResult(nameof(ctx.Amount), message);
                currentNode.AddResult(error);
            }
            if (ctx.Deck != null && String.IsNullOrWhiteSpace(ctx.Deck))
            {
                var message = String.Format(_fmtEmptyWhitespace, nameof(ctx.Deck));
                var error = new StringResult(nameof(ctx.Deck), message);
                currentNode.AddResult(error);
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
