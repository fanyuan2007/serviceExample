using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for HoleLength requests.
    /// </summary>
    public class HoleLengthValidator : ValidatorBase<HoleLength>
    {
        /// <summary>
        /// Instantiates a new HoleLength validator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public HoleLengthValidator(IValidationSettings settings, RequestValidator owner)
            : base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single HoleLength request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context object was not a HoleLength request.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The HoleLength request to validate.</param>
        /// <returns>True if no errors were found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as HoleLength;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (Double.IsNaN(ctx.Total) || 
                ctx.Total < 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThanEqualTo, nameof(ctx.Total), 0.0);
                var error = new StringResult(nameof(ctx.Total), message);
                currentNode.AddResult(error);
            }
            if (Double.IsNaN(ctx.Average) || 
                ctx.Average < 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThanEqualTo, nameof(ctx.Average), 0.0);
                var error = new StringResult(nameof(ctx.Average), message);
                currentNode.AddResult(error);
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
