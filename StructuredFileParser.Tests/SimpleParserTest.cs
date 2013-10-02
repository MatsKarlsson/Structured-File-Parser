using FlatFileParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StructuredFileParser.Tests
{
	[TestClass]
	public class SimpleParserTest
	{

		[TestMethod]
		public void Parse_ParsingLine_ReturnsInstance()
		{
			var parser = new SimpleParser<SimpleEntity>();

			var simpleEntity = parser.ParseLine("Param1;Param2", ";");

			Assert.IsNotNull(simpleEntity);
			Assert.AreEqual(simpleEntity.Param1, "Param1");
			Assert.AreEqual(simpleEntity.Param2, "Param2");
		}
	}

	public class SimpleEntity
	{

		[Parameter(0)]
		public string Param1 { get; set; }
		[Parameter(1)]
		public string Param2 { get; set; }
	}
}