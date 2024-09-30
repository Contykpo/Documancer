namespace Documancer.Application.Common.Models
{
    public class PaginationFilter : BaseFilter
    {
        #region Properties

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public string OrderBy { get; set; } = "Id";
        public string SortDirection { get; set; } = "Descending";

        #endregion

        #region Methods

        public override string ToString()
        {
            return
                $"PageNumber:{PageNumber},PageSize:{PageSize},OrderBy:{OrderBy},SortDirection:{SortDirection},Keyword:{Keyword}";
        }

        #endregion
    }

    public class BaseFilter
    {
        public string? Keyword { get; set; }
    }
}