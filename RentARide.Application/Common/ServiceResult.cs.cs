using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Application.Common
{
    public class ServiceResult<T>
    {
     

            public bool IsSuccess { get; set; }
            public T? Data { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
            public int StatusCode { get; set; } 

            public static ServiceResult<T> Success(T data) => new() { IsSuccess = true, Data = data, StatusCode = 200 };
            public static ServiceResult<T> Failure(string message, int statusCode = 400) => new() { IsSuccess = false, ErrorMessage = message, StatusCode = statusCode };

    }
}
