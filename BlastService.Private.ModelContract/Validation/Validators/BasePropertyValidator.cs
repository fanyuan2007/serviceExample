using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for BaseProperty requests.
    /// </summary>
    public class BasePropertyValidator : ValidatorBase<BaseProperty>
    {
        /// <summary>
        /// Instantiates a new BaseProperty validator with provided settings and
        /// the owning RequestValidator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public BasePropertyValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single BaseProperty request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided object is not a BaseProperty request.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The BaseProperty request to validate.</param>
        /// <returns>True if there were no errors found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as BaseProperty;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (ctx.CreatedAt > ctx.UpdatedAt)
            {
                var message = "Update date must be later than or the same as the creation date.";
                var error = new StringResult(nameof(ctx.CreatedAt), message);
                currentNode.AddResult(error);
            }

            return currentNode.RootResult.ErrorCount == 0;
        }
    }
}
