using System.Net;

namespace Documancer.Application.Common.ExceptionHandlers
{
    public class ServerException : Exception
    {
        #region Properties

        public IEnumerable<string> ErrorMessages { get; }

        public HttpStatusCode StatusCode { get; }

        #endregion

        #region Constructors

        public ServerException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            ErrorMessages = new[] { message };
            StatusCode = statusCode;
        }

        #endregion
    }
}