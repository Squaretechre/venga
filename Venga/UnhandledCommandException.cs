using System;

namespace Venga
{
    public class UnhandledCommandException : Exception
    {
        public UnhandledCommandException(string message) : base(message)
        {
        }
    }
}