using MediatR;
using Microsoft.EntityFrameworkCore;
using SoleCode.Api.Common;
using SoleCode.Api.Dto;
using SoleCode.Api.Entities;
using SoleCode.Api.Handlers.User.Queries;
using System.Security.Claims;

namespace SoleCode.Api.Handlers.User
{
    public class GetUserHandler : IRequestHandler<GetUserByUIDQuery, ApiResponse<UserDto?>>, 
        IRequestHandler<GetUserByUsernameQuery, ApiResponse<UserDto?>>,
        IRequestHandler<GetUserQuery, ApiResponse<List<UserDto>>>
    {
        private readonly EntityDbContext _context;
        private readonly IJwtAuthManager _iJwtAuthManager;
        private readonly ILogger _logger;

        public GetUserHandler(EntityDbContext context, IJwtAuthManager IJwtAuthManager, ILogger<GetUserHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _iJwtAuthManager = IJwtAuthManager ?? throw new ArgumentNullException(nameof(IJwtAuthManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<UserDto?>> Handle(GetUserByUIDQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Got a request content type by UID: {request.UID}");

                var data = await _context.User.AsNoTracking().FirstOrDefaultAsync(e => e.UID == request.UID);
                if (data == null) throw new BadHttpRequestException("User Not Found");

                var claims = new[]
                {
                    new Claim("sub",data.Username),
                    new Claim("uid", data.UID.ToString()),
                    new Claim("username",data.Username),
                };
                var tokeResult = _iJwtAuthManager.GenerateTokens(data.Username, claims, DateTime.Now);
                var user = data.MapToDto();
                user.Token = tokeResult;
                return new ApiResponse<UserDto?>(user); ;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<UserDto?>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Got a request user by Username : {request.Username}");
                var data = await _context.User.AsNoTracking().FirstOrDefaultAsync(e => e.Username == request.Username);
                if (data == null) throw new BadHttpRequestException("User Not Found");

                var claims = new[]
                {
                    new Claim("sub",data.Username),
                    new Claim("uid", data.UID.ToString()),
                    new Claim("username",data.Username),
                };
                var tokeResult = _iJwtAuthManager.GenerateTokens(data.Username, claims, DateTime.Now);
                var user = data.MapToDto();
                user.Token = tokeResult;
                return new ApiResponse<UserDto?>(user); ;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<List<UserDto>>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Got a request user list");
                var user = await _context.User.AsNoTracking().Select(x => x.MapToDto()).ToListAsync();
                return new ApiResponse<List<UserDto>>(user); ;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }
    }
}
