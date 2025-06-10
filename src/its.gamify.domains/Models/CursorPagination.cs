using System.Text.Json.Serialization;

namespace its.gamify.domains.Models;

public class CursorPagination
{
    [JsonPropertyName("total_items_count")]
    public int TotalItemsCount { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("page_index")]
    public int PageIndex { get; set; }

    [JsonPropertyName("total_pages_count")]
    public int TotalPagesCount
    {
        get
        {
            var temp = TotalItemsCount / PageSize;
            return TotalItemsCount % PageSize == 0 ? temp : temp + 1;
        }
    }
    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; set; }

    [JsonPropertyName("previous_cursor")]
    public string? PreviousCursor { get; set; }

    [JsonPropertyName("has_next")]
    public bool HasNext => !string.IsNullOrEmpty(NextCursor);

    [JsonPropertyName("has_previous")]
    public bool HasPrevious => !string.IsNullOrEmpty(PreviousCursor);
}
