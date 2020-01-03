using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// Base class for Fragmentation validators.
    /// </summary>
    public abstract class FragmentationValidator : ValidatorBase<Fragmentation>
    {
        /// <summary>
        /// Instantiates a new Fragmentation validator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public FragmentationValidator(IValidationSettings settings, RequestValidator owner)
            : base(settings, owner)
        {

        }

        /// <summary>
        /// Validates a single Fragmentation request.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context object is not a Fragmentation.</exception>
        /// <param name="result">The validator result.</param>
        /// <param name="context">The Fragmentation request object to validate.</param>
        /// <returns>True if no errors have been found. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as Fragmentation;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            ValidateSinglePValue(ctx.P10, nameof(ctx.P10), ref currentNode);
            ValidateSinglePValue(ctx.P20, nameof(ctx.P20), ref currentNode);
            ValidateSinglePValue(ctx.P30, nameof(ctx.P30), ref currentNode);
            ValidateSinglePValue(ctx.P40, nameof(ctx.P40), ref currentNode);
            ValidateSinglePValue(ctx.P50, nameof(ctx.P50), ref currentNode);
            ValidateSinglePValue(ctx.P60, nameof(ctx.P60), ref currentNode);
            ValidateSinglePValue(ctx.P70, nameof(ctx.P70), ref currentNode);
            ValidateSinglePValue(ctx.P80, nameof(ctx.P80), ref currentNode);
            ValidateSinglePValue(ctx.P90, nameof(ctx.P90), ref currentNode);

            return result.RootResult.ErrorCount == 0;
        }

        /// <summary>
        /// Validates a single PValue entry.
        /// </summary>
        /// <param name="pvalue">The pvalue.</param>
        /// <param name="propertyName">The property name of the pvalue's source.</param>
        /// <param name="result">The validation results.</param>
        protected void ValidateSinglePValue(double? pvalue, string propertyName, ref NodeResult result)
        {
            if (pvalue != null &&
                (Double.IsNaN(pvalue.GetValueOrDefault()) ||
                 pvalue <= 0))
            {
                var message = String.Format(_fmtDoubleGreaterThan, propertyName, 0.0);
                var error = new StringResult(propertyName, message);
                result.AddResult(error);
            }
        }

        /// <summary>
        /// Validates that successive PValues are not less than the previous ones, if not null.
        /// </summary>
        /// <param name="pvalues">The list of pvalues.</param>
        /// <param name="pvalueNames">The names of the pvalues.</param>
        /// <param name="result">The validation results.</param>
        protected void ValidatePValueChain(List<double?> pvalues, List<string> pvalueNames, ref NodeResult result)
        {
            if (pvalues.Count != pvalueNames.Count)
            {
                throw new ArgumentException("pvalues don't have the same count as pvalueNames!");
            }

            for (int i = 0; i < pvalues.Count; i++)
            {
                if (!pvalues[i].HasValue)
                {
                    continue;
                }

                var lessThanIndex = pvalues.FindIndex(pv => pv != null && pv < pvalues[i]);
                if (lessThanIndex >= 0 && lessThanIndex > i)
                {
                    var message = String.Format("PValue '{0}' must not be greater than '{1}'",
                        pvalueNames[lessThanIndex],
                        pvalueNames[i]);
                    var error = new StringResult("PValue chain check", message);
                    result.AddResult(error);
                }
            }
        }
    }
}
