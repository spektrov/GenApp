namespace GenApp.WebApi.Models;

public record ErrorDetails
{
    public ErrorDetails(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    public int StatusCode { get; set; }

    public string Message { get; set; }
}