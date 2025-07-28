using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.DAO.Models
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        // ✅ Success helper
        public static OperationResult Ok(string message = "Operation successful.")
        {
            return new OperationResult
            {
                Success = true,
                Message = message
            };
        }

        // ❌ Failure helper
        public static OperationResult Fail(string message = "Operation failed.")
        {
            return new OperationResult
            {
                Success = false,
                Message = message
            };
        }
    }
}
