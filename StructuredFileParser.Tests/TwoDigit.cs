using System.Collections.Generic;
using System.Diagnostics;
using FlatFileParser;

namespace StructuredFileParser.Tests
{
    [ParseRoot("^(?<Header>\\d{2});.+$")]
    public class TwoDigit
    {
        public Header TitleHeader { get; set; }
        public Address Address { get; set; }
    }

    [ParseRoot("^(?<Header>\\d{2});.+$")]
    public class TwoDigitList
    {
        public string Name { get; set; }

        public List<Address> Addresses { get; set; }

    }

    [ParseFormat("02", "^.+;(?<Title>.+);(?<Name>.+)$")]
    public class Header
    {
        public string Title { get; set; }
        public string Name { get; set; }
    }

    [ParseFormat("03", "^.+;(?<Name>.+);(?<StreetAddress>.+);(?<PoBox>.+)$")]
    [DebuggerDisplay("Address: {Name} - {StreetAddress} - {PoBox}")]
    public class Address
    {
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string PoBox { get; set; }
    }
}
