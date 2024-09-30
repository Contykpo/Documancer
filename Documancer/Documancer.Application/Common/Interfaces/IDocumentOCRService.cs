namespace Documancer.Application.Common.Interfaces
{
    public interface IDocumentOCRService
    {
        void Do(int id);
        Task Recognition(int id, CancellationToken cancellationToken);
    }
}