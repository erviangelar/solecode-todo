using MediatR;
using Newtonsoft.Json;
using SoleCode.Api.Common;
using SoleCode.Api.Dto;
using SoleCode.Api.Entities;
using SoleCode.Api.Handlers.TodoList.Queries;

namespace SoleCode.Api.Handlers.TodoList
{
    public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ApiResponse<bool>>,
        IRequestHandler<UpdateStatusCommand, ApiResponse<bool>>
    {
        private readonly EntityDbContext _context;
        private readonly ILogger _logger;

        public UpdateCommandHandler(EntityDbContext context, ILogger<UpdateCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<bool>> Handle(UpdateCommand data, CancellationToken cancellationToken)
        {
            var item = new Entities.TodoList();
            try
            {
                _logger.LogInformation($"Got a request update todo list");

                if (data.user == null)
                    throw new BadHttpRequestException("User not found");

                item = _context.TodoLists.FirstOrDefault(x => x.UID == data.UID);
                if (item == null)
                    throw new BadHttpRequestException("Todo List not found");

                item.Name = data.request.Name;
                item.Status = data.request.Status;
                item.UpdatedBy = data.user.UID.ToString();
                item.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Update todo list error : {ex}");
                throw new SystemException($"Update todo list error : {ex.Message}");
            }
            finally
            {
                var oldValue = JsonConvert.SerializeObject(item);
                var newValue = JsonConvert.SerializeObject(data.request.MapToEntity());
                _context.TodoListHistory.Add(new TodoListHistory()
                {
                    RowUID = item.UID,
                    OldData = oldValue,
                    NewData = newValue,
                    CreatedBy = data.user.UID.ToString(),
                    CreatedDate = DateTime.Now
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ApiResponse<bool>> Handle(UpdateStatusCommand data, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Got a request update status todo list");

                if (data.user == null)
                    throw new BadHttpRequestException("User not found");

                var item = _context.TodoLists.FirstOrDefault(x => x.UID == data.UID);
                if (item == null)
                    throw new BadHttpRequestException("Todo List not found");

                item.Status = data.status;
                item.UpdatedBy = data.user.UID.ToString();
                item.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Update status todo list error : {ex}");
                throw new SystemException($"Update status todo list error : {ex.Message}");
            }
        }
    }
}
