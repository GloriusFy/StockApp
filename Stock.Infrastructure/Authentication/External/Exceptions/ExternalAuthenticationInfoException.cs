using Stock.Infrastructure.Authentication.External.Models;

namespace Stock.Infrastructure.Authentication.External.Exceptions;

public class ExternalAuthenticationInfoException : Exception
{
    public readonly ExternalUserData ReceivedData;
    public readonly IEnumerable<string> MissingFields;

    public ExternalAuthenticationInfoException(IEnumerable<string> missingFields = null, ExternalUserData receivedData = null)
        : base("External authentication yielded insufficient information to allow local login.")
    {
        ReceivedData = receivedData;
        MissingFields = missingFields;
    }
}