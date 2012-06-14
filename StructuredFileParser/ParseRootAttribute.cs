using System;

namespace FlatFileParser
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ParseRootAttribute : Attribute
    {
        public string Format { get; private set; }

        // This is a positional argument
        public ParseRootAttribute(string format)
        {
            Format = format;
        }
    }
}