using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Results
{
    public class ValidationPasswordResult
    {
        public ValidationPasswordResult(bool isValid, string message = "")
        {
            Message = message;
            IsValid = isValid;
        }
        public string Message { get; private set; }
        public bool IsValid { get; private set; }
    }
}