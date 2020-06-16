namespace MyAppBack.Helpers
{
  public class UserParams
  {
    private const int MaxPageSize = 100000000;
    public int PageIndex { get; set; }
    private int _pageSize = 6;
    public int PageSize
    {
      get { return _pageSize; }
      set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
    }

    public string? sort { get; set; }
    public int? regionId { get; set; }
    public int? typeId { get; set; }
    private string _search { get; set; }
    public string? Search
    {
      get => _search;
      set => _search = value.ToLower();
    }

  }
}