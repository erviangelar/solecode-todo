using System.ComponentModel.DataAnnotations;

namespace SoleCode.Api.Entities
{
    public class User : Base
    {
        [StringLength(124)]
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
