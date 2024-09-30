namespace Documancer.Application.Common.Interfaces
{
    // TODO: Could be improved or removed using MediatR...
    public interface IApplicationHubWrapper
    {
        Task JobStarted(int id, string message);
        Task JobCompleted(int id, string message);
    }
}