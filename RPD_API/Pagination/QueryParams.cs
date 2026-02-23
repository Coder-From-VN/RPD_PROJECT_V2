using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace RPD_API.Pagination
{
    public class QueryParams
    {
        //paginton
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        [JsonIgnore]
        [BindNever]
        public int Skip => (PageNumber - 1) * PageSize;

        // Search
        public string? Search { get; set; }

        // Sorting
        public string? SortBy { get; set; } = "Name";
        public string? SortOrder { get; set; } = "asc";
    }
}
