namespace Repository;

public class ReservePresentException : Exception
{
    public ReservePresentException() : base()
    {
    }

    public ReservePresentException(string message) : base(message)
    {
    }

    public ReservePresentException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}