using FlatFileParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StructuredFileParser.Tests
{
	[TestClass]
	public class FileParserTest_Array
	{

		[TestMethod]
		public void ParseStringAsArray_WhenPassedString_ReturnsArrayOfItems()
		{

			var parser = new RegExParser<ArrayTest>();

			const string data = 
@"Mr.;Bert;45
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



		[TestMethod]
		public void ParseStringAsArray_WithDifferentNamespace_ReturnsArrayOfItems()
		{
			var parser = new RegExParser<OtherNamespace.ArrayTest>();

			const string data = 
@"Mr.;Bert;45
Mrs.;Bertha;40";
			var list = parser.ParseStringAsArray(data);

			Assert.AreEqual(2, list.Count);

			Assert.AreEqual("Mr.", list[0].Title2);
			Assert.AreEqual("Bert", list[0].Name2);
			Assert.AreEqual(45, list[0].Age2);

			Assert.AreEqual("Mrs.", list[1].Title2);
			Assert.AreEqual("Bertha", list[1].Name2);
			Assert.AreEqual(40, list[1].Age2);
		}
	}

	[ParseFormat(null, "^(?<Title>.+);(?<Name>.+);(?<Age>.+)$")]
	public class ArrayTest
	{
		public string Title { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
	}

}

namespace StructuredFileParser.Tests.OtherNamespace
{
	[ParseFormat(null, "^(?<Title2>.+);(?<Name2>.+);(?<Age2>.+)$")]
	public class ArrayTest
	{
		public string Title2 { get; set; }
		public string Name2 { get; set; }
		public int Age2 { get; set; }
	}
}
