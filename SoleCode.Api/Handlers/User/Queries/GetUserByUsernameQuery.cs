using SoleCode.Api.Common;
using SoleCode.Api.Dto;

namespace SoleCode.Api.Handlers.User.Queries
{
    public class GetUserByUsernameQuery : QueryBase<ApiResponse<UserDto?>>
    {
        public GetUserByUsernameQuery(string Username)
        {
            this.Username = Username;
        }
        public string Username { get; set; }
    }
}
