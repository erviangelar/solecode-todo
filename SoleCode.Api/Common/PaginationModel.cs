using Microsoft.AspNetCore.Mvc;

namespace SoleCode.Api.Common
{
    public class PaginationModel
    {
        [FromQuery(Name = "page")]
        public int PageNumber { get; set; }

        [FromQuery(Name = "limit")]
        public int PageSize { get; set; }

        public PaginationModel()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public PaginationModel(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
