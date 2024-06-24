namespace Core.Exceptions
{
    public class InvalidSendTimeException : Exception
    {
        public InvalidSendTimeException() : base("The campaign send time must be in the future.")
        {
        }

        public InvalidSendTimeException(string message) : base(message)
        {
        }

        public InvalidSendTimeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
