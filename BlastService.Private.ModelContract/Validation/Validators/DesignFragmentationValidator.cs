using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for DesignFragmentation requests.
    /// </summary>
    public class DesignFragmentationValidator : FragmentationValidator
    {
        /// <summary>
        /// Instantiates a new DesignFragmentation validator with provided settings
        /// and the owning RequestValidator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public DesignFragmentationValidator(IValidationSettings settings, RequestValidator owner)
            : base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single DesignFragmentation request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the context object is not a DesignFragmentation request.</exception>
        /// <param name="result">The validation results</param>
        /// <param name="context">The DesignFragmentation request to be validated.</param>
        /// <returns>True if there were no errors found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as DesignFragmentation;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            var pvalues = new List<double?>()
            {
                ctx.P10, ctx.P20, ctx.P30,
                ctx.P40, ctx.P50, ctx.P60,
                ctx.P70, ctx.P80, ctx.P90,
            };
            if (pvalues.TrueForAll(pvalue => pvalue == null))
            {
                var message = "At least one of the PValues " +
                              "in a design fragmentation must be non-null.";
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
                };

                ValidatePValueChain(pvalues, pvalueNames, ref currentNode);
            }

            base.Validate(ref result, context);

            return result.RootResult.ErrorCount == 0;
        }

        public override bool CanValidate(object obj)
        {
            return obj is DesignFragmentation;
        }
    }
}
