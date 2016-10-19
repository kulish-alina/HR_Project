using System.Collections;
using System.Collections.Generic;

namespace Mailer.Validation
{
    public class DynamicContentValidator : ICollection<string>
    {
        private List<string> Rules;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicContentValidator"/> class.
        /// </summary>
        public DynamicContentValidator()
        {
            Rules = new List<string>();
        }

        /// <summary>
        /// Checks the content onto matching the rules
        /// </summary>
        /// <param name="text">Dynamic content that should validated</param>
        /// <returns>Result of validation by the rules</returns>
        public DynamicContentValidationResult ValidateContent(string text)
        {
            var validationResult = new DynamicContentValidationResult();
            foreach (string rule in Rules)
            {
                if (!text.Contains(rule))
                {
                    validationResult.Add($"Text should contain the markup {rule}");
                }
            }

            return validationResult;
        }

        #region IEnumerable

        public int Count
        {
            get
            {
                return Rules.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public void Add(string item)
        {
            Rules.Add(item);
        }

        public void Clear()
        {
            Rules.Clear();
        }

        public bool Contains(string item)
        {
            return Rules.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            Rules.CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Rules.GetEnumerator();
        }

        public bool Remove(string item)
        {
            return Rules.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Rules).GetEnumerator();
        }
        #endregion
    }
}
