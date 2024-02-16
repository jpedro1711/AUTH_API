using System.Net;

namespace authAPI
{
    public class ErrorResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }

        public ErrorResponseModel(HttpStatusCode statusCode, string? message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}