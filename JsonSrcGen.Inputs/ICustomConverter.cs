using System;
using System.Text;

namespace JsonSrcGen
{
    internal interface ICustomConverter<T>
    {
        void ToJson(StringBuilder builder, T target);

        void FromJson(ReadOnlySpan<char> span, T value);
    }
}