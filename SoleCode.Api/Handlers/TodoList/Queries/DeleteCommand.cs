using SoleCode.Api.Common;
using SoleCode.Api.Dto;

namespace SoleCode.Api.Handlers.TodoList.Queries
{
    public class DeleteCommand : QueryBase<ApiResponse<bool>>
    {
        public Guid UID { get; set; }

        public DeleteCommand(Guid UID, UserClaim? user = null)
        {
            this.UID = UID;
            this.user = user;
        }
    }
}
