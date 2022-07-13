namespace SoleCode.Api.Common
{
    public class FilterModel
    {
        public string? Keyword { get; set; } = "";
        public string? Sort { get; set; } = "";
        public string? Direction { get; set; } = "DESC";
    }
}
