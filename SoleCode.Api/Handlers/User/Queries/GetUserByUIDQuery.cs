using SoleCode.Api.Common;
using SoleCode.Api.Dto;

namespace SoleCode.Api.Handlers.User.Queries
{
    public class GetUserByUIDQuery : QueryBase<ApiResponse<UserDto?>>
    {
        public GetUserByUIDQuery(Guid UID)
        {
            this.UID = UID;
        }
        public Guid UID { get; set; }
    }
}
