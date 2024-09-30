using System.Net;

namespace Documancer.Application.Common.ExceptionHandlers
{
    public class ForbiddenException : ServerException
    {
        public ForbiddenException(string message) : base(message, HttpStatusCode.Forbidden)
        {
        }
    }
}