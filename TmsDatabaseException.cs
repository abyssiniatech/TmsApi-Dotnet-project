/// <summary>
/// Custom exception for TMS database-related errors
/// </summary>
public class TmsDatabaseException : Exception
{
    public TmsDatabaseException()
    {
    }

    public TmsDatabaseException(string message) : base(message)
    {
    }

    public TmsDatabaseException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
