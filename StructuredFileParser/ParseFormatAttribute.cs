using System;

namespace FlatFileParser
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ParseFormatAttribute : Attribute
    {
        public string RowIdentifier { get; private set; }

        public string Format { get; private set; }

        public string ParentAttributeName { get; private set; }

        public ParseFormatAttribute()
        {
            ParentAttributeName = null;
        }

        public ParseFormatAttribute(string rowIdentifier, string format)
        {
            RowIdentifier = rowIdentifier;
            Format = format;
            ParentAttributeName = null;
        }

        public ParseFormatAttribute(string rowIdentifier, string format, string parentAttributeName)
        {
            RowIdentifier = rowIdentifier;
            Format = format;
            ParentAttributeName = parentAttributeName;
        }
    }
}