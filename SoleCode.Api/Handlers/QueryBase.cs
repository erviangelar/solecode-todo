using MediatR;
using SoleCode.Api.Common;
using SoleCode.Api.Dto;

namespace SoleCode.Api.Handlers
{
    public abstract class QueryBase<TResult> : IRequest<TResult> where TResult : class
    {
        public UserClaim? user { get; set; }
    }
}
