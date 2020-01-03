using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for Scoring requests.
    /// </summary>
    public class ScoringValidator : ValidatorBase<Scoring>
    {
        /// <summary>
        /// Instantiates a new Scoring validator with provided settings
        /// and the owning RequestValidator.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="owner"></param>
        public ScoringValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single Scoring request
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the context is not a ScoringRequest.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The Scoring request to validate.</param>
        /// <returns>True if there were no errors found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as Scoring;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (ctx.MetricScore == null)
            {
                var message = String.Format(_fmtNull, nameof(ctx.MetricScore));
                var error = new StringResult(nameof(ctx.MetricScore), message);
                currentNode.AddResult(error);
            }

            if (ctx.MetricScore != null && ctx.MetricScore.Count() < 1)
            {
                var message = string.Format(_fmtCollectionItemCountAtLeast, nameof(ctx.MetricScore), 1);
                var error = new StringResult(nameof(ctx.MetricScore), message);
                currentNode.AddResult(error);
            }

            if (Double.IsNaN(ctx.TotalScore))
            {
                var message = String.Format(_fmtDoubleValid, nameof(ctx.TotalScore));
                var error = new StringResult(nameof(ctx.TotalScore), message);
                currentNode.AddResult(error);
            }

            if (ctx.MetricScore != null)
            {
                int index = 0;
                var childListNode = new NodeResult(nameof(ctx.MetricScore));
                currentNode.AddResult(childListNode);

                foreach (var metric in ctx.MetricScore)
                {
                    if (Settings.MaxErrors > 0 && 
                        result.RootResult.ErrorCount >= Settings.MaxErrors)
                    {
                        break;
                    }

                    IValidatorResult childItemNode = new NodeResult(index.ToString());
                    childListNode.AddResult(childItemNode);

                    Owner.Validate(metric, ref childItemNode);
                    index++;
                }
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
