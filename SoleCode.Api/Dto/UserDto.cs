using SoleCode.Api.Entities;

namespace SoleCode.Api.Dto
{
    public class UserDto
    {
        public Guid UID { get; set; }
        public string Username { get; set; } = "";
        public string Token { get; set; } = "";
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class UserFlter
    {
        public string Username { get; set; } = "";
    }

    public static class UserMapper
    {
        public static UserDto MapToDto(this Entities.User item)
        {
            return new UserDto
            {
                UID = item.UID,
                Username = item.Username
            };
        }
    }
}
