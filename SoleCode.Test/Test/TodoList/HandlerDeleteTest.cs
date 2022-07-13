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
    public class HandlerDeleteTest : IClassFixture<InitTest>
    {
        private InitTest _feature;
        private readonly ILogger<DeleteCommandHandler> _logger;
        public HandlerDeleteTest(InitTest feature)
        {
            _feature = feature;
            _logger = A.Fake<ILogger<DeleteCommandHandler>>();
        }

        [Fact]
        public void Init()
        {
            var handler = new DeleteCommandHandler(_feature._context, _logger);
            handler.ShouldNotBeNull();
        }

        [Fact]
        public async Task DeleteCommandShouldBeOK()
        {
            var data = new TodoListBuilder().WithUid().WithName().build();
            _feature._context.TodoLists.Add(data);
            await _feature._context.SaveChangesAsync();

            var handler = new DeleteCommandHandler(_feature._context, _logger);
            var resultRequest = new DeleteCommand(data.UID, new UserClaim() { UID = Guid.NewGuid(), Username = "Test" });

            var action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            action.Data.ShouldBe(true);
        }
    }
}
