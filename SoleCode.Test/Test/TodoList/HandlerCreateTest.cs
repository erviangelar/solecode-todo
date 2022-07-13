using FakeItEasy;
using Microsoft.Extensions.Logging;
using Shouldly;
using SoleCode.Api.Common;
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
    public class HandlerCreateTest : IClassFixture<InitTest>
    {
        private InitTest _feature;
        private readonly ILogger<CreateCommandHandler> _logger;
        public HandlerCreateTest(InitTest feature)
        {
            _feature = feature;
            _logger = A.Fake<ILogger<CreateCommandHandler>>();
        }

        [Fact]
        public void Init()
        {
            var handler = new CreateCommandHandler(_feature._context, _logger);
            handler.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateCommandShouldBeOK()
        {
            var data = new TodoListRequestBuilder().WithName("demo").build();

            var handler = new CreateCommandHandler(_feature._context, _logger);
            var resultRequest = new CreateCommand(data, new UserClaim(){ UID = Guid.NewGuid(), Username = "Test" });

            var action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            action.Data.ShouldNotBe(Guid.Empty);
        }
    }
}
