using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for ChargeInfo requests.
    /// </summary>
    public class ChargeInfoValidator : ValidatorBase<ChargeInfo>
    {
        /// <summary>
        /// Instantiates a new ChargeInfo validator with provided settings
        /// and owning RequestValidator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public ChargeInfoValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single ChargeInfo request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context object is not a ChargeInfo request.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The ChargeInfo request object to validate.</param>
        /// <returns>True if there were no errors found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as ChargeInfo;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (Double.IsNaN(ctx.Weight) || ctx.Weight < 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThanEqualTo, nameof(ctx.Weight), 0.0);
                var error = new StringResult(nameof(ctx.Weight), message);
                currentNode.AddResult(error);
            }
            if (Double.IsNaN(ctx.Thickness) || ctx.Thickness < 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThanEqualTo, nameof(ctx.Thickness), 0.0);
                var error = new StringResult(nameof(ctx.Thickness), message);
                currentNode.AddResult(error);
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
