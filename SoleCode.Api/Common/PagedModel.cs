namespace SoleCode.Api.Common
{
    public class PagedModel<T>
    {
        public class MetaData
        {
            public int? PageNumber { get; set; }

            public int? PageSize { get; set; }

            public int Total { get; set; }
        }

        public IEnumerable<T> Data { get; set; }

        public MetaData Meta { get; set; }

        public PagedModel()
        {
            Data = new List<T>();
            Meta = new MetaData();
        }

        public PagedModel(IEnumerable<T> data)
        {
            Data = data;
            Meta = new MetaData();
        }
    }
}
