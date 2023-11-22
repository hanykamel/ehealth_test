using FluentValidation.Results;

namespace EHealth.ManageItemLists.Domain.Shared.Exceptions;
public class DataNotValidException : Exception
{
    public int StatusCode { get; set; }
    public List<ValidationFailure>? Errors { get; set; }
    public string? HttpResponseMessage { get; set; }
    public DataNotValidException(string? message = "The data not valid", List<ValidationFailure>? errors = null) : base(message)
    {
        HttpResponseMessage = message;
        Errors = errors;
        StatusCode = 422;
    }
}