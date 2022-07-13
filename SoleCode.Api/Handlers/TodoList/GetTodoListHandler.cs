using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoleCode.Api.Common;
using SoleCode.Api.Dto;
using SoleCode.Api.Entities;
using SoleCode.Api.Handlers.TodoList.Queries;
using System.Linq.Dynamic.Core;

namespace SoleCode.Api.Handlers.TodoList
{
    public class GetTodoListHandler : IRequestHandler<GetTodoListQuery, PagedModel<TodoListDto>>
    {
        private readonly EntityDbContext _context;
        private readonly ILogger _logger;

        public GetTodoListHandler(EntityDbContext context, ILogger<GetTodoListHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PagedModel<TodoListDto?>> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Got a request todo list");

                var predicate = PredicateBuilder.New<Entities.TodoList>(true);
                if (!string.IsNullOrEmpty(request.filter?.Keyword))
                {
                    predicate.And(x => x.Name.ToLower().Contains(request.filter.Keyword.ToLower()));
                }
                var query = _context.TodoLists.Where(predicate);

                if (!string.IsNullOrEmpty(request.filter?.Sort))
                {
                    if (request.filter.Sort == "Name")
                    {
                        query = query.OrderBy($"Name {request.filter.Direction}");
                    }
                    else if (request.filter.Sort == "Status")
                    {
                        query = query.OrderBy($"Status {request.filter.Direction}");
                    }
                    else if (request.filter.Sort == "Date")
                    {
                        query = query.OrderBy($"CreatedBy {request.filter.Direction}");
                    }
                    else
                    {
                        query = query.OrderBy($"UID {request.filter.Direction}");
                    }
                }

                var Paged = new PagedModel<TodoListDto>(new List<TodoListDto>());
                var paging = request.paging;

                Paged.Meta.Total = await query.CountAsync().ConfigureAwait(true);
                query = query.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize);
                Paged.Meta.PageNumber = paging.PageNumber;
                Paged.Meta.PageSize = paging.PageSize;
                Paged.Data = await query.Select(x => x.MapToDto()).ToListAsync(cancellationToken);
                return Paged;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed Get a request todo list ex: {ex.Message}");
                throw;
            }
        }
    }
}
