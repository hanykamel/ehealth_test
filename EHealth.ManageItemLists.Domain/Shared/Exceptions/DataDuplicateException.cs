using FluentValidation.Results;

namespace EHealth.ManageItemLists.Domain.Shared.Exceptions;
public class DataDuplicateException : Exception
{
    public int StatusCode { get; set; }
    public string? HttpResponseMessage { get; set; }
    public List<ValidationFailure>? Errors { get; set; }
    public DataDuplicateException(string message = "The data was duplicated", List<ValidationFailure>? errors = null) : base(message)
    {
        HttpResponseMessage = message;
        StatusCode = 409 ;
        Errors = errors;
    }
}