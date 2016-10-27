using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Results
{
    public class ChangePasswordResult
    {
        public ChangePasswordResult(bool result, string message = null)
        {
            Message = message ?? "";
            Result = result;
        }
        public string Message { get; private set; }
        public bool Result { get; private set; }
    }
}