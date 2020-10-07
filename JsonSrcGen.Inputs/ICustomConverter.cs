using System;
using System.Text;

namespace JsonSrcGen
{
    internal interface ICustomConverter<T> where T : class
    {
        void ToJson(IJsonBuilder builder, T target);

        ReadOnlySpan<char> FromJson(ReadOnlySpan<char> span, ref T value);
    }
}