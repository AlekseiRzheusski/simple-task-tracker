namespace SimpleTaskTracker.Exceptions;

public class IdNotFoundInDatabase : Exception
{
    public IdNotFoundInDatabase(string message)
        : base(message) { }
}