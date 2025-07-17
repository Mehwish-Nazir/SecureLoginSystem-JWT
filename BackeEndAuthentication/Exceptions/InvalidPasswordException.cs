namespace BackeEndAuthentication.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException()
            : base("Invalid password") //  Proper default message
        {
        }

        public InvalidPasswordException(string? message)
            : base(message)
        {
        }
    }
}
