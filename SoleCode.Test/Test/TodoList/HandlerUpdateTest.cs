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
    public class HandlerUpdateTest : IClassFixture<InitTest>
    {
        private InitTest _feature;
        private readonly ILogger<UpdateCommandHandler> _logger;
        public HandlerUpdateTest(InitTest feature)
        {
            _feature = feature;
            _logger = A.Fake<ILogger<UpdateCommandHandler>>();
        }

        [Fact]
        public void Init()
        {
            var handler = new UpdateCommandHandler(_feature._context, _logger);
            handler.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateCommandShouldBeOK()
        {
            var data = new TodoListBuilder().WithUid().WithName().WithStatus().build();
            _feature._context.TodoLists.Add(data);
            await _feature._context.SaveChangesAsync();

            var request = new TodoListUpdateRequestBuilder().WithName().WithStatus().build();

            var handler = new UpdateCommandHandler(_feature._context, _logger);
            var resultRequest = new UpdateCommand(data.UID, request, new UserClaim() { UID = Guid.NewGuid(), Username = "Test" });

            var action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            action.Data.ShouldBe(true);
        }

        [Fact]
        public async Task UpdateStatusCommandShouldBeOK()
        {
            var data = new TodoListBuilder().WithUid().WithName().WithStatus().build();
            _feature._context.TodoLists.Add(data);
            await _feature._context.SaveChangesAsync();

            var handler = new UpdateCommandHandler(_feature._context, _logger);
            var resultRequest = new UpdateStatusCommand(data.UID, Const.Status.InProgress, new UserClaim() { UID = Guid.NewGuid(), Username = "Test" });

            var action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            action.Data.ShouldBe(true);
        }
    }
}
