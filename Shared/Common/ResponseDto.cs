using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Common
{
    public class ResponseDto<T>
    {

        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public static ResponseDto<T> Success(T data, int statusCode)
        {
            return new ResponseDto<T>() { Data = data, StatusCode = statusCode };
        }
        public static ResponseDto<T> Success(int statusCode)
        {
            return new ResponseDto<T>() { StatusCode = statusCode };
        }
        public static ResponseDto<T> Fail(string error, int statusCode)
        {
            return new ResponseDto<T>() { Errors = { error }, StatusCode = statusCode };
        }

        public static ResponseDto<T> Fail(List<string> errors, int statusCode)
        {
            return new ResponseDto<T>() { Errors = errors, StatusCode = statusCode };
        }

    }
}
