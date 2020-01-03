using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract.Validation
{
    public class StringResult : IValidatorResult
    {
        #region Lifecycle
        public StringResult(string objectName, string message)
        {
            _objectName = objectName;
            _message = message;

            // Format the message.
            IValidatorResult currentResult = this;
            
        }
        #endregion
        #region Fields and Properties
        private IValidatorResult _parentResult;
        private IValidatorResult _rootResult;
        private readonly string _objectName;
        private readonly string _message;
        #endregion
        #region IValidatorResult
        public IEnumerable<IValidatorResult> ChildResults
        {
            get
            {
                throw new NotSupportedException("String results cannot have children.");
            }
        }    

        /// <summary>
        /// Gets the parent result.
        /// </summary>
        public IValidatorResult ParentResult
        {
            get { return _parentResult; }
            set 
            { 
                _parentResult = value;

                // Reset the root result.
                _rootResult = this;
                while (_rootResult.ParentResult != null)
                {
                    _rootResult = _rootResult.ParentResult;
                }
            }
        }

        /// <summary>
        /// Gets the root result.
        /// </summary>
        public IValidatorResult RootResult
        {
            get { return _rootResult; }
        }

        /// <summary>
        /// Always returns 1 as this is an error result without children.
        /// </summary>
        public int ErrorCount
        {
            get
            {
                return 1;
            }
        }

        public string ObjectName
        {
            get { return _objectName; }
        }

        public string Message
        {
            get { return _message; }
        }

        public string FormattedMessage
        {
            get 
            {
                IValidatorResult currentResult = this;
                var formattedMessage = Message;
                while (currentResult != null)
                {
                    formattedMessage = String.Format("{0} : {1}", currentResult.ObjectName, formattedMessage);
                    currentResult = currentResult.ParentResult;
                }

                return formattedMessage;
            }
        }
        #endregion
    }
}
