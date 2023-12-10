namespace Domain.Base.PaginageDto;

public class PaginateList<T>
{
    public PaginateList(IEnumerable<T> data, int count, int pageSize)
    {
        Data = data;
        TotalPages = count / pageSize;
        if (count % pageSize != 0)
            TotalPages++;
    }

    public IEnumerable<T> Data { get; set; }
    public int TotalPages { get; set; }
}
