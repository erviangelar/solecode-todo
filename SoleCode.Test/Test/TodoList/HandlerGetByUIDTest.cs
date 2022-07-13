using FakeItEasy;
using Microsoft.Extensions.Logging;
using Shouldly;
using SoleCode.Api.Handlers.TodoList;
using SoleCode.Api.Handlers.TodoList.Queries;
using SoleCode.Test.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SoleCode.Test.Test.TodoList
{
    public class HandlerGetByUIDTest : IClassFixture<InitTest>
    {
        private InitTest _feature;
        private readonly ILogger<GetTodoListByUIDHandler> _logger;
        public HandlerGetByUIDTest(InitTest feature)
        {
            _feature = feature;
            _logger = A.Fake<ILogger<GetTodoListByUIDHandler>>();
        }

        [Fact]
        public void Init()
        {
            var handler = new GetTodoListByUIDHandler(_feature._context, _logger);
            handler.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetByUIDShouldBeOK()
        {

            var data = new TodoListBuilder().WithUid().WithName().WithStatus().build();
            _feature._context.TodoLists.Add(data);
            _feature._context.SaveChanges();

            var handler = new GetTodoListByUIDHandler(_feature._context, _logger);
            var resultRequest = new GetTodoListByUIDQuery(data.UID);

            var action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            action.Data.ShouldNotBeNull();
        }
    }
}
