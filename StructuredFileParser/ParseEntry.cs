using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FlatFileParser
{

    /// <summary>
    /// Information about a Class that can be created when parsing file. How to parse line, Properties on class and what type to instanciate.
    /// </summary>
    public class ParseEntry
    {
        public Type Type { get; set; }
        public Regex ParseFormat { get; set; }
        public string ParentAttributeName { get; set; }

        /// <summary>
        /// Set first time ParseEntry is parsed (not when initially loaded, but first time Type is instanciated)
        /// </summary>
        public PropertyInfo[] PropertyInfos { get; set; }

    }
}