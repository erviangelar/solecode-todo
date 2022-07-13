using SoleCode.Api.Common;
using SoleCode.Api.Dto;

namespace SoleCode.Api.Handlers.User.Queries
{
    public class GetUserQuery : QueryBase<ApiResponse<List<UserDto>>>
    {
        public GetUserQuery()
        {
        }
    }
}
