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
    public class HandlerGetTest : IClassFixture<InitTest>
    {
        private InitTest _feature;
        private readonly ILogger<GetTodoListHandler> _logger;
        public HandlerGetTest(InitTest feature)
        {
            _feature = feature;
            _logger = A.Fake<ILogger<GetTodoListHandler>>();
        }

        [Fact]
        public void Init()
        {
            var handler = new GetTodoListHandler(_feature._context, _logger);
            handler.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetShouldBeMoreThanZeroOK()
        {
            for (int i = 0; i < 5; i++)
            {
                var data = new TodoListBuilder().WithUid().WithName(i.ToString()).WithStatus().build();
                _feature._context.TodoLists.Add(data);
                _feature._context.SaveChanges();
            }

            var handler = new GetTodoListHandler(_feature._context, _logger);
            var resultRequest = new GetTodoListQuery(
                new PaginationModel() { PageNumber = 1, PageSize = 10}, 
                new FilterModel() { Keyword = null , Sort = null, Direction = null},
                new UserClaim() { UID = Guid.NewGuid() , Username = "" }
                );

            var action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            action.Data.ShouldNotBeNull();
            action.Data.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetWithFilterShouldBeMoreThanZeroOK()
        {

            var data = new TodoListBuilder().WithUid().WithName("demofilter").WithStatus().build();
            _feature._context.TodoLists.Add(data);
            _feature._context.SaveChanges();

            var handler = new GetTodoListHandler(_feature._context, _logger);
            var resultRequest = new GetTodoListQuery(
                new PaginationModel() { PageNumber = 1, PageSize = 10 },
                new FilterModel() { Keyword = data.Name, Sort = null, Direction = null },
                new UserClaim() { UID = Guid.NewGuid(), Username = "" }
                );

            var action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            action.Data.ShouldNotBeNull();
            action.Data.Count().ShouldBe(1);
        }


        [Fact]
        public async Task GetWithSortShouldBeMoreThanZeroOK()
        {
            for (int i = 0; i < 5; i++)
            {
                var data = new TodoListBuilder().WithUid().WithName($"demo{i}").WithStatus().build();
                _feature._context.TodoLists.Add(data);
                _feature._context.SaveChanges();
            }

            var handler = new GetTodoListHandler(_feature._context, _logger);
            var resultRequest = new GetTodoListQuery(
                new PaginationModel(1, 10),
                new FilterModel() { Keyword = null, Sort = "Name", Direction = "ASC" },
                new UserClaim() { UID = Guid.NewGuid(), Username = "" }
                );
            var action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            //action.Data.FirstOrDefault()?.Name.ShouldBe($"Test demo{0}");

            resultRequest = new GetTodoListQuery(
                new PaginationModel(1, 10),
                new FilterModel() { Keyword = null, Sort = "Status", Direction = "DESC" },
                new UserClaim() { UID = Guid.NewGuid(), Username = "" }
                );
            action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            //action.Data.FirstOrDefault()?.Name.ShouldBe($"Test demo{0}");

            resultRequest = new GetTodoListQuery(
                new PaginationModel(1, 10),
                new FilterModel() { Keyword = null, Sort = "Date", Direction = "DESC" },
                new UserClaim() { UID = Guid.NewGuid(), Username = "" }
                );
            action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            //action.Data.FirstOrDefault()?.Name.ShouldBe($"Test demo{0}");
        }
    }
}
