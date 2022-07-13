namespace SoleCode.Api.Entities
{
    public class TodoListHistory
    {
        public Guid UID { get; set; }
        public Guid RowUID { get; set; }
        public string? NewData { get; set; }
        public string? OldData { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = "";
    }
}
