using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for Transformation requests.
    /// </summary>
    public class TransformationValidator : ValidatorBase<Transformation>
    {
        /// <summary>
        /// Instantiates a new Transformation validator with provided settings and
        /// the owning RequestValidator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public TransformationValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {

        }

        /// <summary>
        /// Validates a single Transformation request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the context is not a Transformation object.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The Transformation object to validate.</param>
        /// <returns>True if there were no errors found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as Transformation;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (Double.IsNaN(ctx.East))
            {
                var message = String.Format(_fmtDoubleValid, nameof(ctx.East));
                var error = new StringResult(nameof(ctx.East), message);
                currentNode.AddResult(error);
            }
            if (Double.IsNaN(ctx.North))
            {
                var message = String.Format(_fmtDoubleValid, nameof(ctx.North));
                var error = new StringResult(nameof(ctx.North), message);
                currentNode.AddResult(error);
            }
            if (Double.IsNaN(ctx.Elev))
            {
                var message = String.Format(_fmtDoubleValid, nameof(ctx.Elev));
                var error = new StringResult(nameof(ctx.Elev), message);
                currentNode.AddResult(error);
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
