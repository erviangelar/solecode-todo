using FakeItEasy;
using Microsoft.Extensions.Logging;
using Shouldly;
using SoleCode.Api.Common;
using SoleCode.Api.Handlers.User;
using SoleCode.Api.Handlers.User.Queries;
using SoleCode.Test.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SoleCode.Test.Test.User
{
    public class HandlerGetUserTest : IClassFixture<InitTest>
    {
        private InitTest _feature;
        private readonly ILogger<GetUserHandler> _logger;
        private readonly IJwtAuthManager _jwtmanager;
        public HandlerGetUserTest(InitTest feature)
        {
            _feature = feature;
            _logger = A.Fake<ILogger<GetUserHandler>>();
            _jwtmanager = A.Fake<IJwtAuthManager>();
        }

        [Fact]
        public void Init()
        {
            var handler = new GetUserHandler(_feature._context, _jwtmanager, _logger);
            handler.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetShouldBeMoreThanZeroOK()
        {
            for (int i = 0; i < 5; i++)
            {
                var data = new UserBuilder().WithUid().WithUsername(i.ToString()).build();
                _feature._context.User.Add(data);
                _feature._context.SaveChanges();
            }

            var handler = new GetUserHandler(_feature._context, _jwtmanager, _logger);
            var resultRequest = new GetUserQuery();

            var action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            action.Data.ShouldNotBeNull();
            action.Data.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetByUsernameShouldBeOK()
        {
            var data = new UserBuilder().WithUid().WithUsername().build();
            _feature._context.User.Add(data);
            _feature._context.SaveChanges();

            var handler = new GetUserHandler(_feature._context, _jwtmanager, _logger);
            var resultRequest = new GetUserByUsernameQuery(data.Username);

            var action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            action.Data.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetByUIDShouldBeOK()
        {
            var data = new UserBuilder().WithUid().WithUsername().build();
            _feature._context.User.Add(data);
            _feature._context.SaveChanges();

            var handler = new GetUserHandler(_feature._context, _jwtmanager, _logger);
            var resultRequest = new GetUserByUIDQuery(data.UID);

            var action = await handler.Handle(resultRequest, CancellationToken.None);
            action.ShouldNotBeNull();
            action.Data.ShouldNotBeNull();
        }
    }
}
