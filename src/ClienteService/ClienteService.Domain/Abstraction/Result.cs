using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClienteService.Domain.Abstraction
{
    public class Result<TDto> where TDto : IResult
    {
        // Sucess
        private Result(
            TDto? data,
            int statusCode)
        {
            Data = data;
            IsNotSuccessfull = false;
            StatusCode = statusCode;
        }

        // Success without data
        private Result(int statusCode)
        {
            IsNotSuccessfull = false;
            StatusCode = statusCode;
        }

        // Fail with one error
        private Result(
            int statusCode,
            List<string> errorMessage)
        {
            IsNotSuccessfull = true;
            StatusCode = statusCode;
            Error = errorMessage;
        }

        public TDto? Data { get; set; }
        [JsonIgnore]
        public bool IsNotSuccessfull { get; set; }
        public int StatusCode { get; set; }
        public List<string> Error { get; set; } = null!;

        public static Result<TDto> Success(
            TDto data,
            int statusCode)
            => new(data, statusCode);

        // Success without data
        public static Result<TDto> Success(
            int statusCode)
            => new(statusCode);

        // Fail with one error
        public static Result<TDto> Failed(
            int statusCode,
            string errorMessage)
            => new(statusCode, [errorMessage]);

        // Fail with more errors
        public static Result<TDto> Failed(
            int statusCode,
            List<string> errorMessage)
            => new(statusCode, errorMessage);
    }

    public class NoContentDto : IResult;
}
