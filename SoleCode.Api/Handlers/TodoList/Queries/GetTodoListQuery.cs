using SoleCode.Api.Common;
using SoleCode.Api.Dto;

namespace SoleCode.Api.Handlers.TodoList.Queries
{
    public class GetTodoListQuery : QueryBase<PagedModel<TodoListDto>>
    {
        public PaginationModel? paging { get; set; }
        public FilterModel? filter { get; set; }
        public GetTodoListQuery(PaginationModel? paging, FilterModel? filter = null, UserClaim? user = null)
        {
            this.paging = paging;
            this.filter = filter;
            this.user = user;
        }
    }
}
