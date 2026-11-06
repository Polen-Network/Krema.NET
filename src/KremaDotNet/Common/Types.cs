using System.Text.Json.Serialization;

namespace KremaDotNet.Common;

/// <summary>
/// Page information returned by list endpoints.
/// </summary>
public class PaginationEntity
{
    [JsonPropertyName("total_records")]
    public required int TotalRecords { get; set; }

    [JsonPropertyName("total_pages")]
    public required int TotalPages { get; set; }

    [JsonPropertyName("current_page")]
    public required int CurrentPage { get; set; }

    [JsonPropertyName("next_page")]
    public int? NextPage { get; set; }

    [JsonPropertyName("prev_page")]
    public int? PrevPage { get; set; }
}
