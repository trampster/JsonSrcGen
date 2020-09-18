using System;

namespace JsonSrcGen
{
    public class InvalidJsonException : Exception
    {
        public InvalidJsonException(string message) : base(message)
        {

        }
    }
}