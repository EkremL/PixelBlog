using System;

namespace API.Exceptions;

//! Custom exception class for handling application-specific errors
public class ApiException : Exception
{
    //? The HTTP status code to be returned (e.g., 404, 400, 401, etc.)
    public int StatusCode { get; set; }

    //? Override the base Message property to access the error message from Exception
    public override string Message => base.Message;

    //? Optional field to store detailed error information (e.g., stack trace or custom message)
    public string? Details { get; set; }

    //* Constructor to initialize the status code, message, and optional details
    public ApiException(int statusCode, string? message = null, string? details = null) : base(message)
    {
        StatusCode = statusCode;
        Details = details;
    }
}