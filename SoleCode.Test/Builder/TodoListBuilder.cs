using Fluency;
using SoleCode.Api.Common;
using SoleCode.Api.Dto;
using SoleCode.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoleCode.Test.Builder
{
    public class TodoListBuilder : FluentBuilder<TodoList>
    {
        public TodoListBuilder WithUid(Guid? uid = null)
        {
            SetProperty(x => x.UID, uid ?? Guid.NewGuid());
            return this;
        }
        public TodoListBuilder WithName(string name = "")
        {
            SetProperty(x => x.Name, $"Test {name}");
            return this;
        }
        public TodoListBuilder WithStatus(string status = "")
        {
            SetProperty(x => x.Status, status ?? Const.Status.Ready);
            return this;
        }
    }
    public class TodoListRequestBuilder : FluentBuilder<TodoListRequest>
    {
        public TodoListRequestBuilder WithName(string name = "")
        {
            SetProperty(x => x.Name, $"Test {name}");
            return this;
        }
    }
    public class TodoListUpdateRequestBuilder : FluentBuilder<TodoListUpdateRequest>
    {
        public TodoListUpdateRequestBuilder WithName(string name = "")
        {
            SetProperty(x => x.Name, $"Test {name}");
            return this;
        }
        public TodoListUpdateRequestBuilder WithStatus(string status = "")
        {
            SetProperty(x => x.Status, status ?? Const.Status.Ready);
            return this;
        }
    }
}
