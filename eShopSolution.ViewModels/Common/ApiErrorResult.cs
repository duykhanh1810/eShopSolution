using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public string[] ValidationError { get; set; } //trong trường hợp nhiều message

        public ApiErrorResult()
        {
        }

        public ApiErrorResult(string message)
        {
            IsSuccess = false;
            Message = message;
        }

        public ApiErrorResult(string[] validationError)
        {
            IsSuccess = false;
            ValidationError = validationError;
        }
    }
}