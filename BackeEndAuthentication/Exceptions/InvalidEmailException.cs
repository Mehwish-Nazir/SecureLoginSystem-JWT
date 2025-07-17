namespace BackeEndAuthentication.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException()
            : base("Invalid email or account is disabled")
        {
        }

        public InvalidEmailException(string? message)
            : base(message)
        {
        }
    }
}
