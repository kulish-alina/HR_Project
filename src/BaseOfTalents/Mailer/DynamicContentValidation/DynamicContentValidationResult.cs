using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mailer.Validation
{
    public class DynamicContentValidationResult : ICollection<string>
    {
        public List<string> _errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicContentValidationResult"/> class.
        /// </summary>
        public DynamicContentValidationResult()
        {
            _errors = new List<string>();
        }

        /// <summary>
        /// Is result of validation acceptable (valid)
        /// </summary>
        public bool IsAcceptable
        {
            get
            {
                return !_errors.Any();
            }
        }

        /// <summary>
        /// Paragraphed errors in one string
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return _errors.Aggregate("", (s, x) => s += $"{x}\r\n");
            }
        }

        #region IEnumerable
        public void Add(string errorMessage)
        {
            _errors.Add(errorMessage);
        }

        public void Clear()
        {
            _errors.Clear();
        }

        public bool Contains(string item)
        {
            return _errors.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _errors.CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            return _errors.Remove(item);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_errors).GetEnumerator();
        }

        public int Count
        {
            get
            {
                return _errors.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
        #endregion
    }
}
