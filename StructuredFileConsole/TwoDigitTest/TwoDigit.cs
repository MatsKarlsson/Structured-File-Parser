using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using FlatFileParser;
namespace FlatFileConsole.TwoDigitTest
{

    [ParseRoot("^(?<Header>\\d{2});.+$")]
    [XmlRoot(ElementName = "File")]
    public class TwoDigit
    {
        public Header TitleHeader { get; set; }
        public Address Address { get; set; }
        //public SubHeader12 SubHeader12 { get; set; }
    }

    [ParseRoot("^(?<Header>\\d{2});.+$")]
    public class TwoDigitList
    {
        public Header TitleHeader { get; set; }

        public Header OtherHeader { get; set; }

        public List<Address> Addresses { get; set; }

    }

    [ParseFormat("01", "^.+;(?<Title>.+);(?<Name>.+)$", "TitleHeader")]
    [ParseFormat("02", "^.+;(?<Name>.+),(?<Title>.+),(?<ExtraInfo>.+)$", "OtherHeader")]
	[DebuggerDisplay("Title: {Title}, Name: {Name}, Extra: {ExtraInfo}")]
    public class Header
    {
        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string ExtraInfo { get; set; }
    }

    [ParseFormat("03", "^.+;(?<Name>.+);(?<StreetAddress>.+);(?<PoBox>.+)$")]
    [DebuggerDisplay("{Name} - {StreetAddress} - {PoBox} - {SubHeader12}")]
    public class Address
    {
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string PoBox { get; set; }

        public SubHeader12 SubHeader12 { get; set; }
    }

    [ParseFormat("13", "^.+;(?<SubHeader>.+);(?<MoreInfo>.+)$")]
    [DebuggerDisplay("SubHeader: {SubHeader}, MoreInfo: {MoreInfo}")]
    public class SubHeader12
    {
        public string SubHeader { get; set; }
        public string MoreInfo { get; set; }
    }
}
