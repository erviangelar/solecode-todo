using SoleCode.Api.Common;
using SoleCode.Api.Dto;

namespace SoleCode.Api.Handlers.TodoList.Queries
{
    public class CreateCommand : QueryBase<ApiResponse<Guid>>
    {
        public TodoListRequest request { get; set; }

        public CreateCommand(TodoListRequest request, UserClaim? user = null)
        {
            this.request = request;
            this.user = user;
        }
    }
}
