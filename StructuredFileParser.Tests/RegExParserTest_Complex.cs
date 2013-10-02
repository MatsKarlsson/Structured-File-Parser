using System.IO;
using System.Text;
using FlatFileParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StructuredFileParser.Tests
{
	[TestClass]
	public class RegExParserTest_Complex
	{
		[TestMethod]
		public void Parse_ParsingFile_ReturnsComplexObjectGraph()
		{
			var parser = new RegExParser<ComplexTestRoot>();

			const string data = 
@"01;HeaderTitle;HeaderName
02;Item1;Description1;More1
02;Item2;Description2;More2
";
			using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
			{
				var complexTestRoot = parser.Parse(ms);

				Assert.IsNotNull(complexTestRoot);
				Assert.AreEqual("HeaderTitle", complexTestRoot.Header.Title);
				Assert.AreEqual("HeaderName", complexTestRoot.Header.Name);
				Assert.AreEqual("HeaderName", complexTestRoot.Header.Name);
				Assert.AreEqual(2, complexTestRoot.Items.Count);
				Assert.AreEqual("Item1", complexTestRoot.Items[0].Name);
				Assert.AreEqual("Description1", complexTestRoot.Items[0].Description);
				Assert.AreEqual("More1", complexTestRoot.Items[0].More);
			}
		}

		[TestMethod]
		public void Parse_MultipleEqualRowIdentifiersInDifferentNamespaces_ReturnsComplexObjectGraph()
		{
			var parser = new RegExParser<OtherNamespace.OtherComplexTestRoot>();

			const string data = @"
01;OtherTitle;OtherName";
			using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
			{
				var otherComplexTestRoot = parser.Parse(ms);

				Assert.IsNotNull(otherComplexTestRoot);
				Assert.AreEqual("OtherTitle", otherComplexTestRoot.Item.Title);
				Assert.AreEqual("OtherName", otherComplexTestRoot.Item.Name);
			}
		}
	
		//TODO: Add test for multiple items with same multiple parseformats and parent-attribute names
	}
}
