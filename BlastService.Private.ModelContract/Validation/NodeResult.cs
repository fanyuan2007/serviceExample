using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation
{
    /// <summary>
    /// A result which may have one or more results attached to it.
    /// Typically represents a class or structure.
    /// </summary>
    public class NodeResult : IValidatorResult
    {
        #region Lifecycle
        /// <summary>
        /// Creates a new NodeResult with a given name.
        /// </summary>
        /// <param name="objectName">The name of this node.</param>
        public NodeResult(string objectName)
        {
            _objectName = objectName;
            _childResults = new List<IValidatorResult>();
            ParentResult = null;
        }
        #endregion
        #region Fields and Properties
        private readonly string _objectName;
        private IValidatorResult _parentResult;
        private IValidatorResult _rootResult;
        private readonly List<IValidatorResult> _childResults;
        #endregion
        #region Functions
        /// <summary>
        /// Appends a new validation result to this node's list of results.
        /// </summary>
        /// <param name="result"></param>
        public void AddResult(IValidatorResult result)
        {
            result.ParentResult = this;
            _childResults.Add(result);
        }
        #endregion
        #region IValidatorResult
        /// <summary>
        /// Retrieves an enumeration of all the validation results added to this node.
        /// </summary>
        public IEnumerable<IValidatorResult> ChildResults
        {
            get { return _childResults; }
        }

        /// <summary>
        /// The node that is one level above this node.
        /// </summary>
        public IValidatorResult ParentResult
        {
            get 
            { 
                return _parentResult; 
            }
            set
            {
                _parentResult = value;

                // Get the root result.
                _rootResult = this;
                while (_rootResult.ParentResult != null)
                {
                    _rootResult = _rootResult.ParentResult;
                }
            }
        }

        /// <summary>
        /// Gets a list of all child results in the tree and flattens them into a list.
        /// </summary>
        /// <returns></returns>
        public List<IValidatorResult> GetAllResultsAsList()
        {
            var results = new List<IValidatorResult>();

            results.AddRange(ChildResults.Select(cr => cr as NodeResult)
                                         .Where(cr => cr != null)
                                         .SelectMany(cr => cr.GetAllResultsAsList()));
            results.AddRange(ChildResults.Where(cr => !(cr is NodeResult)));

            return results;
        }

        /// <summary>
        /// The top-level node. If there is no parent,
        /// this is the top level node.
        /// </summary>
        public IValidatorResult RootResult
        {
            get 
            {
                return _rootResult;
            }
        }

        /// <summary>
        /// Gets the total number of errors in the children of this node.
        /// </summary>
        public int ErrorCount
        {
            get
            {
                return _childResults.Any() 
                    ? _childResults.Sum(res => res.ErrorCount) 
                    : 0;
            }
        }

        /// <summary>
        /// Gets the name of this node.
        /// </summary>
        public string ObjectName
        {
            get { return _objectName; }
        }

        /// <summary>
        /// Gets the error message on this node.
        /// Not valid for nodes
        /// </summary>
        public string Message
        {
            // Nodes don't have messages.
            get { return null; }
        }

        public string FormattedMessage
        {
            // Nodes don't have messages.
            get { return null; }
        }
        #endregion
    }
}
