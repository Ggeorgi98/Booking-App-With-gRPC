using System.Text.Json.Serialization;

namespace BookingApp.Users.Domain
{
    public class PagedResults<T>
    {
        public int CurrentPage { get; }

        public int PageSize { get; }

        public int TotalPagesCount { get; }

        public long TotalResultsCount { get; }

        public ICollection<T> Result { get; }

        public PagedResults()
        {

        }

        [JsonConstructor]
        public PagedResults(ICollection<T> result,
            int currentPage, int pageSize,
            int totalPagesCount, long totalResultsCount)
        {
            Result = result;
            CurrentPage = currentPage > totalPagesCount
                ? totalPagesCount
                : currentPage;
            PageSize = pageSize;
            TotalPagesCount = totalPagesCount;
            TotalResultsCount = totalResultsCount;
        }
    }
}
