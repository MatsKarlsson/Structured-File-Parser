using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FlatFileParser
{
    public class ParseEntries : IParseEntries
    {
        private readonly Dictionary<string, ParseEntry> _parseEntries = new Dictionary<string, ParseEntry>();


        public void Clear()
        {
            _parseEntries.Clear();
        }

        public void Load(Type rootNode)
        {
            //Cache all types with ParseFormat-attributes
            var types = rootNode.Assembly.GetTypes();
            foreach (var type in types)
            {
                var attrs = type.GetCustomAttributes(typeof(ParseFormatAttribute), false);
                foreach (ParseFormatAttribute attr in attrs)
                    _parseEntries.Add(attr.RowIdentifier, new ParseEntry { Type = type, ParseFormat = new Regex(attr.Format), ParentAttributeName = attr.ParentAttributeName });
            }
        }

        public bool ContainsKey(string key)
        {
            return _parseEntries.ContainsKey(key);
        }

        public ParseEntry this[string key]
        {
            get { return _parseEntries[key]; }
        }
    }
}