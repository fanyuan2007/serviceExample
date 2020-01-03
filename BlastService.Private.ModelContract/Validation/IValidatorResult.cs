using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract.Validation
{
    public interface IValidatorResult
    {
        IEnumerable<IValidatorResult> ChildResults { get; }
        IValidatorResult ParentResult { get; set; }
        IValidatorResult RootResult { get; }
        int ErrorCount { get; }
        string ObjectName { get; }
        string Message { get; }
        string FormattedMessage { get; }
    }
}
