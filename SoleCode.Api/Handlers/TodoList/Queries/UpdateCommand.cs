using SoleCode.Api.Common;
using SoleCode.Api.Dto;

namespace SoleCode.Api.Handlers.TodoList.Queries
{
    public class UpdateCommand : QueryBase<ApiResponse<bool>>
    {
        public Guid UID { get; set; }
        public TodoListUpdateRequest request { get; set; }

        public UpdateCommand(Guid UID, TodoListUpdateRequest request, UserClaim? user = null)
        {
            this.UID = UID;
            this.request = request;
            this.user = user;
        }
    }

    public class UpdateStatusCommand : QueryBase<ApiResponse<bool>>
    {
        public Guid UID { get; set; }
        public string status { get; set; }

        public UpdateStatusCommand(Guid UID, string status, UserClaim? user = null)
        {
            this.UID = UID;
            this.status = status;
            this.user = user;
        }
    }
}
