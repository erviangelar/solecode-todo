using System.Net;

namespace SoleCode.Api.Common;

    public class ErrorCode
    {
        public ErrorCode(string? code, string message)
        {
            Code = code;
            Message = message;
        }
        public ErrorCode(string? code, string message, HttpStatusCode statusCode)
        {
            Code = code;
            Message = message;
            StatusCode = statusCode;
        }
        public string? Code { get; private set; }
        public string Message { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
    }
    public static  class ErrorCodes 
    {
        public static ErrorCode Forbidden
        {
            get
            {
                return new ErrorCode("U001", "Forbidden", HttpStatusCode.Forbidden);
            }
        }

        public static ErrorCode DurationTimeout
        {
            get
            {
                return new ErrorCode("D001", "Run out of time", HttpStatusCode.BadRequest);
            }
        }
        public static ErrorCode InvalidToken
        {
            get
            {
                return new ErrorCode("U002", "Invalid Token", HttpStatusCode.Unauthorized);
            }
        }
        public static ErrorCode BadRequest
        {
            get
            {
                return new ErrorCode("400", "BadRequest", HttpStatusCode.BadRequest);
            }
        }
    }