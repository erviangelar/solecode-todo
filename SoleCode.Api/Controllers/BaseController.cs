using Microsoft.AspNetCore.Mvc;
using SoleCode.Api.Common;
using SoleCode.Api.Dto;
using System.Security.Claims;

namespace SoleCode.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        protected UserClaim GetUserClaim()
        {
            try
            {
                if (User == null)
                    throw new Exception();
                var uid = User.FindFirst("uid");
                if (uid == null || uid.Value == null)
                    throw new Exception();
                var username = User.FindFirst("username");
                if (username == null || username.Value == null)
                    throw new Exception();
                return new UserClaim
                {
                    UID = Guid.Parse(uid.Value),
                    Username = username.Value
                };
            }
            catch
            {
                throw new UnauthorizedAccessException("Invalid Token");
            }

        }
    }
}
