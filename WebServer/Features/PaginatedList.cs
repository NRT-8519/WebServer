using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;

namespace WebServer.Features
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalItems { get; private set; }
        public int PageSize { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalItems = count;
            PageSize = pageSize;
            TotalPages = (int) Math.Ceiling(TotalItems / (double) PageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (source != null)
            {
                int count = await source.CountAsync();
                var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                return new PaginatedList<T>(items, count, pageIndex, pageSize);
            }
            else
            {
                return new PaginatedList<T>(new List<T>(), 0, pageIndex, pageSize);
            }
        }
    }
}
