namespace cs_project.Core.Models
{
    public class PagingQueryParameters
    {
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; }
    }
}
