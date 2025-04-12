using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Errors.ProfileError
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public static OperationResult Success(string? message = null)
            => new OperationResult { IsSuccess = true, Message = message };

        public static OperationResult Failure(string message)
            => new OperationResult { IsSuccess = false, Message = message };
    }
}
