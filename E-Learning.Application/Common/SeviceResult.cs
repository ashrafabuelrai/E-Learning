using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Application.Common
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string? ErrorMessage { get; private set; }
        public int StatusCode { get; private set; }

        public static ServiceResult<T> Success(T data, int statusCode = 200)
            => new() { IsSuccess = true, Data = data, StatusCode = statusCode };

        public static ServiceResult<T> Failure(string errorMessage, int statusCode = 400)
            => new() { IsSuccess = false, ErrorMessage = errorMessage, StatusCode = statusCode };

        public static ServiceResult<T> NotFound(string message = "Resource not found.")
            => new() { IsSuccess = false, ErrorMessage = message, StatusCode = 404 };

        public static ServiceResult<T> Forbidden(string message = "You are not authorized to perform this action.")
            => new() { IsSuccess = false, ErrorMessage = message, StatusCode = 403 };
    }

}
