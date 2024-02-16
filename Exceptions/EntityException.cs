using System.Net;

namespace authAPI 
{
    public class EntityException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public EntityException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}