using Fluency;
using SoleCode.Api.Dto;
using SoleCode.Api.Entities;

namespace SoleCode.Test.Builder
{
    public class UserBuilder : FluentBuilder<User>
    {
        public UserBuilder WithUid(Guid? uid = null)
        {
            SetProperty(x => x.UID, uid ?? Guid.NewGuid());
            return this;
        }
        public UserBuilder WithUsername(string name = "")
        {
            SetProperty(x => x.Username, $"Test{name}");
            return this;
        }
    }
}
