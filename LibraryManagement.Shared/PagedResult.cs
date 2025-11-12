using System.Collections;

namespace LibraryManagement.Shared;

public class PagedResult<T>: IEnumerable
{
    public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items.ToList();
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public static PagedResult<T> Create(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
    }
    public IEnumerator GetEnumerator()
    {
        return Items.GetEnumerator();
    }
}