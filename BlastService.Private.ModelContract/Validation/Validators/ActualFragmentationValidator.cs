using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// Class for validating ActualFragmentation objects.
    /// </summary>
    public class ActualFragmentationValidator : FragmentationValidator
    {
        /// <summary>
        /// Instantiates a new ActualFragmentation validator.
        /// </summary>
        /// <param name="settings">The validation settings to use for validation.</param>
        /// <param name="owner">The RequestValidator to attach this to.</param>
        public ActualFragmentationValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single ActualFragmentation request.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context object is not an ActualFragmentation request.</exception>
        /// <param name="result">The list of errors found.</param>
        /// <param name="context">The ActualFragmentation to validate.</param>
        /// <returns>True if no errors were found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as ActualFragmentation;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            ValidateSinglePValue(ctx.TopSize, nameof(ctx.TopSize), ref currentNode);

            var pvalues = new List<double?>()
            {
                ctx.P10, ctx.P20, ctx.P30,
                ctx.P40, ctx.P50, ctx.P60,
                ctx.P70, ctx.P80, ctx.P90,
                ctx.TopSize
            };
            if (pvalues.TrueForAll(pvalue => pvalue == null))
            {
                var message = "At least one of the PValues or TopSize " +
                              "in an actual fragmentation must be non-null.";
                var error = new StringResult("Other", message);
                currentNode.AddResult(error);
            }

            // Non-null PValues should follow the rule: 0 <= P10 <= ... <= P90 <= TopSize
            if (pvalues.Count(pValue => pValue != null) >= 2)
            {
                var pvalueNames = new List<string>()
                {
                    nameof(ctx.P10), nameof(ctx.P20), nameof(ctx.P30),
                    nameof(ctx.P40), nameof(ctx.P50), nameof(ctx.P60),
                    nameof(ctx.P70), nameof(ctx.P80), nameof(ctx.P90),
                    nameof(ctx.TopSize),
                };

                ValidatePValueChain(pvalues, pvalueNames, ref currentNode);
            }

            base.Validate(ref result, context);

            return result.ErrorCount == 0;
        }

        /// <summary>
        /// Evaluates whether an object can be validated by this validator.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <returns>True if the object can be validated. False otherwise.</returns>
        public override bool CanValidate(object obj)
        {
            return obj is ActualFragmentation;
        }
    }
}
