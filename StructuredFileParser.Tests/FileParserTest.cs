using FlatFileParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StructuredFileParser.Tests
{
    [TestClass]
    public class FileParserTest
    {

        [TestMethod]
        public void ParseStringAsArray_WhenPassedString_ReturnsArrayOfItems()
        {

            var parser = new FileParser<ArrayTest>();

            var data = @"Mr.;Bert;45
Mrs.;Bertha;40";
            var list = parser.ParseStringAsArray(data);

            Assert.AreEqual(2, list.Count);

            Assert.AreEqual("Mr.", list[0].Title);
            Assert.AreEqual("Bert", list[0].Name);
            Assert.AreEqual(45, list[0].Age);

            Assert.AreEqual("Mrs.", list[1].Title);
            Assert.AreEqual("Bertha", list[1].Name);
            Assert.AreEqual(40, list[1].Age);
        }
    }


    [ParseFormat(null, "^(?<Title>.+);(?<Name>.+);(?<Age>.+)$")]
    public class ArrayTest
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public int Age{ get; set; }
    }

}
