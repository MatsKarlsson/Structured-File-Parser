using System;

namespace FlatFileParser
{
    public interface IParseEntries
    {
        void Clear();
        void Load(Type rootNode);
        bool ContainsKey(string key);
        ParseEntry this[string key] { get; }
    }
}