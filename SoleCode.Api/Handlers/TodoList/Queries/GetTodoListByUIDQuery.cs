using SoleCode.Api.Common;
using SoleCode.Api.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoleCode.Api.Handlers.TodoList.Queries
{
    public class GetTodoListByUIDQuery : QueryBase<ApiResponse<TodoListDto?>>
    {
        public GetTodoListByUIDQuery(Guid UID)
        {
            this.UID = UID;
        }

        [Required]
        public Guid UID { get; set; }
    }
}
