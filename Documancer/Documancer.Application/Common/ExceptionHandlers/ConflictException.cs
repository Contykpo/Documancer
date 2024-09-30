using System.Net;

namespace Documancer.Application.Common.ExceptionHandlers
{
    public class ConflictException : ServerException
    {
        public ConflictException(string message) : base(message, HttpStatusCode.Conflict)
        {
        }
    }
}