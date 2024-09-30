namespace Documancer.Application.Common.Models
{
    public abstract class PaginationRequest
    {
        #region Properties

        public string? FilterRules { get; set; }
        public int Page { get; set; } = 1;
        public int Rows { get; set; } = 15;
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"page:{Page},rows:{Rows},sort:{Sort},order:{Order},filterRule:{FilterRules}";
        }

        #endregion
    }
}