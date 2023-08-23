namespace WebServer.Models.DTOs
{
    public class PaginatedResultDTO<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int? TotalItems { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }

        public List<T> items { get; set; } = new();
    }
}
