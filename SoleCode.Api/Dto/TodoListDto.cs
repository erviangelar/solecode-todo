namespace SoleCode.Api.Dto
{
    public class TodoListDto
    {
        public Guid UID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class TodoListRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class TodoListUpdateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class TodoListstatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }

    public static class TodoListMapper
    {
        public static TodoListDto MapToDto(this Entities.TodoList item)
        {
            return new TodoListDto
            {
                UID = item.UID,
                Name = item.Name,
                Status = item.Status
            };
        }
        public static Entities.TodoList MapToEntity(this Dto.TodoListRequest item)
        {
            return new Entities.TodoList
            {
                Name = item.Name
            };
        }
        public static Entities.TodoList MapToEntity(this Dto.TodoListUpdateRequest item)
        {
            return new Entities.TodoList
            {
                Name = item.Name,
                Status = item.Status
            };
        }
    }
}
