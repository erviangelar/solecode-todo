using MediatR;
using SoleCode.Api.Common;
using SoleCode.Api.Dto;
using SoleCode.Api.Entities;
using SoleCode.Api.Handlers.TodoList.Queries;
using Newtonsoft.Json;

namespace SoleCode.Api.Handlers.TodoList
{
    public class CreateCommandHandler : IRequestHandler<CreateCommand, ApiResponse<Guid>>
    {
        private readonly EntityDbContext _context;
        private readonly ILogger _logger;

        public CreateCommandHandler(EntityDbContext context, ILogger<CreateCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<Guid>> Handle(CreateCommand data, CancellationToken cancellationToken)
        {
            var item = new Entities.TodoList();
            try
            {
                _logger.LogInformation($"Got a request add todo list");

                if(data.user == null) 
                    throw new BadHttpRequestException("User not found");

                if (_context.TodoLists.Any(x => x.Name.ToLower() == data.request.Name.ToLower() && x.Status != Const.Status.Done))
                    throw new BadHttpRequestException($"Todo list with name {data.request.Name} already exist");

                item = data.request.MapToEntity();
                item.Status = Const.Status.Ready;
                item.CreatedBy = data.user.UID.ToString();
                item.CreatedDate = DateTime.Now;
                _context.TodoLists.Add(item);
                await _context.SaveChangesAsync();
                return new ApiResponse<Guid>(item.UID);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Create todo list error : {ex}");
                throw new SystemException($"Create todo list error : {ex.Message}");
            }
            finally
            {
                var newValue = JsonConvert.SerializeObject(item);
                _context.TodoListHistory.Add(new TodoListHistory() {
                    RowUID = item.UID,
                    NewData = newValue,
                    CreatedBy = data.user.UID.ToString(),
                    CreatedDate = DateTime.Now
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
