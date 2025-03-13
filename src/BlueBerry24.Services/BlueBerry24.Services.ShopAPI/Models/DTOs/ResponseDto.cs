namespace BlueBerry24.Services.ShopAPI.Models.DTOs
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object Data { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
