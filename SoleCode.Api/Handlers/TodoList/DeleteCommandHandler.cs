using MediatR;
using Newtonsoft.Json;
using SoleCode.Api.Common;
using SoleCode.Api.Entities;
using SoleCode.Api.Handlers.TodoList.Queries;

namespace SoleCode.Api.Handlers.TodoList
{
    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ApiResponse<bool>>
    {
        private readonly EntityDbContext _context;
        private readonly ILogger _logger;

        public DeleteCommandHandler(EntityDbContext context, ILogger<DeleteCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<bool>> Handle(DeleteCommand data, CancellationToken cancellationToken)
        {
            var item = new Entities.TodoList();
            try
            {
                _logger.LogInformation($"Got a request delete todo list");

                if (data.user == null)
                    throw new BadHttpRequestException("User not found");

                item = _context.TodoLists.FirstOrDefault(x => x.UID == data.UID);
                if (item == null)
                    throw new BadHttpRequestException("Todo List not found");

                _context.TodoLists.Remove(item);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Delete todolist error : {ex}");
                throw new SystemException($"Delete todo list error : {ex.Message}");
            }
            finally
            {
                var oldValue = JsonConvert.SerializeObject(item);
                _context.TodoListHistory.Add(new TodoListHistory()
                {
                    RowUID = item.UID,
                    OldData = oldValue,
                    CreatedBy = data.user.UID.ToString(),
                    CreatedDate = DateTime.Now
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
