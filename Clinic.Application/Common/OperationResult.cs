using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Clinic.Application.Common
{
    public class OperationResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; } = string.Empty;

        private OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static OperationResult Ok(string message = "") => new(true, message);
        public static OperationResult Fail(string message) => new(false, message);
    }
}

