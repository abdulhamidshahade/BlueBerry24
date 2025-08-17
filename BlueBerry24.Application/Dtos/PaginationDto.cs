namespace BlueBerry24.Application.Dtos
{
    public class PaginationDto<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public int FirstItemOnPage { get; set; }
        public int LastItemOnPage { get; set; }

        public PaginationDto(IEnumerable<T> data, int pageNumber, int pageSize, int totalCount)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            HasPreviousPage = pageNumber > 1;
            HasNextPage = pageNumber < TotalPages;
            FirstItemOnPage = ((pageNumber - 1) * pageSize) + 1;
            LastItemOnPage = Math.Min(pageNumber * pageSize, totalCount);
        }
    }
}