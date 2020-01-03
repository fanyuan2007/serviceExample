using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract.Validation
{
    public interface IValidationSettings
    {
        int MaxErrors { get; }
    }

    /// <summary>
    /// Settings for validation operations.
    /// </summary>
    public sealed class ValidationSettings : IValidationSettings
    {
        /// <summary>
        /// The maximum errors to be found in validation operations before breaking out.
        /// </summary>
        public int MaxErrors
        {
            get { return _maxErrors; }
            set { _maxErrors = Math.Max(0, value); }
        }
        private int _maxErrors;
    }
}
