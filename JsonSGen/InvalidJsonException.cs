using System;

namespace JsonSGen
{
    public class InvalidJsonException : Exception
    {
        public InvalidJsonException(string message) : base(message)
        {

        }
    }
}