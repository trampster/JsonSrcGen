using System;

namespace JsonSG
{
    public class InvalidJsonException : Exception
    {
        public InvalidJsonException(string message) : base(message)
        {

        }
    }
}