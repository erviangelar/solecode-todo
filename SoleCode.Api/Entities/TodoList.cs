using SoleCode.Api.Common;
using System.ComponentModel.DataAnnotations;

namespace SoleCode.Api.Entities
{
    public class TodoList : Base
    {
        [StringLength(256)]
        public string Name { get; set; } = "";
        public string? Status { get; set; } = Const.Status.Ready;
    }
}
