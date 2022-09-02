using Serilog.Core;

namespace HotelListing.Controllers;

public static class CommonErrorMessages
{
    public static string InvalidSubmittedData => "Submitted data is invalid.";
    public static void LogInvalidRequestAttempt(ILogger logger, string method, string objectName) => logger.LogError($"Invalid {method} request attempt in {objectName}");
}