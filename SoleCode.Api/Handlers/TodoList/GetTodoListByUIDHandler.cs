using MediatR;
using Microsoft.EntityFrameworkCore;
using SoleCode.Api.Common;
using SoleCode.Api.Dto;
using SoleCode.Api.Entities;
using SoleCode.Api.Handlers.TodoList.Queries;

namespace SoleCode.Api.Handlers.TodoList
{
    public class GetTodoListByUIDHandler : IRequestHandler<GetTodoListByUIDQuery, ApiResponse<TodoListDto?>>
    {
        private readonly EntityDbContext _context;
        private readonly ILogger _logger;

        public GetTodoListByUIDHandler(EntityDbContext context, ILogger<GetTodoListByUIDHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<TodoListDto?>> Handle(GetTodoListByUIDQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Got a request partner type by UID: {request.UID}");
            var data = await _context.TodoLists.FirstOrDefaultAsync(e => e.UID == request.UID);
            if (data != null) return new ApiResponse<TodoListDto?>(data.MapToDto());
            return new ApiResponse<TodoListDto?>(null); ;
        }
    }
}
