namespace BackeEndAuthentication.CustomExceptions
{
    //add custome exception in middleware file 
    public class ConflictException :Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
