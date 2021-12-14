using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContratosMVVM.Auxiliares
{
    public class OperationResponse
    {
        public OperationResponse(bool isSuccessful, string errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public OperationResponse(bool isSuccessful, string errorMessage, Exception exception)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public bool IsSuccessful { get; }
        public string ErrorMessage { get; }
        public Exception Exception { get; }
    }
}
