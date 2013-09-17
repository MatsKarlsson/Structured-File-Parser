using System;

namespace FlatFileParser
{
    public interface IParseEntries
    {
        void Clear();
        void Load(Type rootNode);
        bool ContainsRowIdentifier(string key);
        ParseEntry this[string key] { get; }
    }
}