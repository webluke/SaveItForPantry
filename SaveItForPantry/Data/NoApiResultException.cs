using System;

namespace SaveItForPantry.Data
{
    public class NoApiResultException : Exception
    {
        public NoApiResultException(string message) : base(message)
        {
        }
    }
}
