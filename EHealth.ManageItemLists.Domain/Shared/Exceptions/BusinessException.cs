namespace EHealth.ManageItemLists.Domain.Shared.Exceptions
{
    public class BusinessException : Exception
    {
        public int StatusCode { get; set; }
        public string? HttpResponseMessage { get; set; }
        public BusinessException(string message = "There was an error processing your request") : base(message)
        {
            StatusCode = 405;
            HttpResponseMessage = message;
        }
    }
}
