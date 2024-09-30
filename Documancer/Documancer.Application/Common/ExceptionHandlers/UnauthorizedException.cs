using System.Net;

namespace Documancer.Application.Common.ExceptionHandlers
{
    public class UnauthorizedException : ServerException
    {
        public UnauthorizedException(string message) : base(message, HttpStatusCode.Unauthorized)
        {
        }
    }
}