using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for LocalTransformation requests.
    /// </summary>
    public class LocalTransformationValidator : ValidatorBase<LocalTransformation>
    {
        /// <summary>
        /// Instantiates a new LocalTransformation validator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public LocalTransformationValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single LocalTransformation request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context object is not a LocalTransformation request.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The LocalTransformation request to validate.</param>
        /// <returns>True if there were no errors found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as LocalTransformation;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (ctx.Translation == null)
            {
                var message = String.Format(_fmtNull, nameof(ctx.Translation));
                var error = new StringResult(nameof(ctx.Translation), message);
                currentNode.AddResult(error);
            }

            if (ctx.Rotation == null)
            {
                var message = String.Format(_fmtNull, nameof(ctx.Rotation));
                var error = new StringResult(nameof(ctx.Rotation), message);
                currentNode.AddResult(error);
            }

            if (ctx.Scaling == null)
            {
                var message = String.Format(_fmtNull, nameof(ctx.Scaling));
                var error = new StringResult(nameof(ctx.Scaling), message);
                currentNode.AddResult(error);
            }

            if (ctx.Translation != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.Translation));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.Translation, ref childNode);
            }

            if (ctx.Rotation != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.Rotation));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.Rotation, ref childNode);
            }

            if (ctx.Scaling != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.Scaling));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.Scaling, ref childNode);
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
