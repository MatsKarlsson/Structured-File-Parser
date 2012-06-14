using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
            var parseEntry = new ParseEntry { ParseFormat = new Regex("^.+;(?<Name>.+);(?<StreetAddress>.+);(?<PoBox>.+)$"), Type = typeof (Address) };

            var parseType = parser.ParseLine(parseEntry, "03;AddressRow1;AddressXX;PoBoxYY");

            Assert.IsInstanceOfType(parseType.Instance, typeof(Address));
        }

        [TestMethod]
        public void ParseLine_ParsingAddressParseEntry_SetStringPropertiesOnInstance()
        {
            var parser = new Parser();
            var parseEntry = new ParseEntry { ParseFormat = new Regex("^.+;(?<Name>.+);(?<StreetAddress>.+);(?<PoBox>.+)$"), Type = typeof(Address) };

            var parseType = parser.ParseLine(parseEntry, "03;TheName;AddressXX;PoBoxYY");

            var address = parseType.Instance as Address;

            Assert.IsNotNull(address);
            Assert.AreEqual(address.Name, "TheName");
            Assert.AreEqual(address.StreetAddress, "AddressXX");
            Assert.AreEqual(address.PoBox, "PoBoxYY");
        }

    }
}
