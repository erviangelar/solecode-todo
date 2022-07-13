namespace SoleCode.Api.Common
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public T Data { get; set; }
        public int? ErrorCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(T data)
        {
            Data = data;
            Message = "";
            ErrorCode = null;
            Status = true;
        }
    }
}
