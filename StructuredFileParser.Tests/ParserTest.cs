using System.Text.RegularExpressions;
using FlatFileParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StructuredFileParser.Tests
{
    [TestClass]
    public class ParserTest
    {

        [TestMethod]
        public void ParseLine_ParsingAddressParseEntry_ReturnsAddressInstance()
        {

            var parser = new Parser();
            var parseEntry = new ParseEntry { ParseFormat = new Regex("^.+;(?<Name>.+);(?<Description>.+);(?<More>.+)$"), Type = typeof (ComplexTestItems) };

            var parseType = parser.ParseLine(parseEntry, "03;AddressRow1;AddressXX;PoBoxYY");

            Assert.IsInstanceOfType(parseType.Instance, typeof(ComplexTestItems));
        }

        [TestMethod]
        public void ParseLine_ParsingAddressParseEntry_SetStringPropertiesOnInstance()
        {
            var parser = new Parser();
            var parseEntry = new ParseEntry { ParseFormat = new Regex("^.+;(?<Name>.+);(?<Description>.+);(?<More>.+)$"), Type = typeof(ComplexTestItems) };

            var parseType = parser.ParseLine(parseEntry, "03;TheName;AddressXX;PoBoxYY");

            var address = parseType.Instance as ComplexTestItems;

            Assert.IsNotNull(address);
            Assert.AreEqual(address.Name, "TheName");
            Assert.AreEqual(address.Description, "AddressXX");
            Assert.AreEqual(address.More, "PoBoxYY");
        }

    }
}
