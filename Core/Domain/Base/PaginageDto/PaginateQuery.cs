#nullable disable

namespace Domain.Base.PaginageDto;

public class PaginateQuery
{
    private int _page;
    private int _pageSize;
    private string _search;

    public int Page
    {
        get => _page < 1 ? 1 : _page;
        set => _page = value;
    }

    public int PageSize
    {
        get => _pageSize < 1 ? 20 : _pageSize;
        set => _pageSize = value;
    }

    public string Search
    {
        get => _search?.ToLower().Trim();
        set => _search = value;
    }
}
