using Microsoft.EntityFrameworkCore;

namespace CalendarSynchronizerWeb.Models
{
    public class CalendarPaginatedList<T>:List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public CalendarPaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static CalendarPaginatedList<T> CreateAsync(List<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count;
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new CalendarPaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
