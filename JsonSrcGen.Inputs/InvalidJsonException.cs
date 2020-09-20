using System;

namespace JsonSrcGen
{
    internal class InvalidJsonException : Exception
    {
        public InvalidJsonException(string message) : base(message)
        {

        }
    }
}