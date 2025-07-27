using System;

namespace ImaginedWorlds.Infrastructure.Exceptions;

public class InvalidAiResponseException : Exception
{
    public string RawAiResponse { get; }

    public InvalidAiResponseException(string message, Exception innerException, string rawAiResponse)
        : base(message, innerException)
    {
        RawAiResponse = rawAiResponse;
    }
}