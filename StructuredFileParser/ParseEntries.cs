using System;
using System.Collections.Generic;
using System.Linq;
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

                var attrs = type.GetCustomAttributes(typeof(ParseFormatAttribute), false) as ParseFormatAttribute[] ?? new ParseFormatAttribute[0];
                foreach (ParseFormatAttribute attr in attrs.Where(d=>d.RowIdentifier != null))
                    _parseEntries.Add( type.Namespace + "." + attr.RowIdentifier,  new ParseEntry
	                    {
		                    Type = type, 
							ParseFormat = new Regex(attr.Format), 
							ParentAttributeName = attr.ParentAttributeName
	                    });
            }
        }

        public bool ContainsRowIdentifier(string key)
        {
            return _parseEntries.ContainsKey(key);
        }

        public ParseEntry this[string key]
        {
            get { return _parseEntries[key]; }
        }
    }
}