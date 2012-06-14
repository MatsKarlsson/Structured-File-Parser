using System;

namespace FlatFileParser
{
    public interface IParser
    {
        ObjectInstance ParseLine(ParseEntry parseEntry, string line);
        bool TryParseGenericList(ParseEntry parseEntry, ObjectInstance newObjectInstance, ObjectInstance objectInstance);
        ObjectInstance LoadRoot(Type rootNode);
        string GetKeyFromLine(string line);
    }
}