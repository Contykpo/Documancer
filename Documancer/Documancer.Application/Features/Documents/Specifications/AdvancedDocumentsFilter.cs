using Documancer.Application.Common.Security;

namespace Documancer.Application.Features.Documents.Specifications
{
    public enum DocumentListView
    {
        [Description("All")] All,
        [Description("My Document")] My,
        [Description("Created Today")] CreatedToday,

        [Description("Created within the last 30 days")]
        Created30Days
    }

    public class AdvancedDocumentsFilter : PaginationFilter
    {
        public int LocalTimezoneOffset { get; set; }
        public DocumentListView ListView { get; set; } = DocumentListView.All;
        public UserProfile? CurrentUser { get; set; }
    }
}