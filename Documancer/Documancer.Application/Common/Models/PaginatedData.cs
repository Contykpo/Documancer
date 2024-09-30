namespace Documancer.Application.Common.Models
{
    public class PaginatedData<T>
    {
        #region Properties

        public int CurrentPage { get; }
        public int TotalItems { get; private set; }
        public int TotalPages { get; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public IEnumerable<T> Items { get; set; }

        #endregion

        #region Constructors

        public PaginatedData(IEnumerable<T> items, int total, int pageIndex, int pageSize)
        {
            Items = items;
            TotalItems = total;
            CurrentPage = pageIndex;
            TotalPages = (int)Math.Ceiling(total / (double)pageSize);
        }

        #endregion

        #region Methods

        public static async Task<PaginatedData<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedData<T>(items, count, pageIndex, pageSize);
        }

        #endregion
    }
}