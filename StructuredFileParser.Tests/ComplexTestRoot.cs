using System.Collections.Generic;
using System.Diagnostics;
using FlatFileParser;

namespace StructuredFileParser.Tests
{
    [ParseRoot("^(?<Header>\\d{2});.+$")]
    public class ComplexTestRoot
    {
	    public	ComplexTestHeader Header { get; set; }
        public List<ComplexTestItems> Items { get; set; }
    }

    [ParseFormat("01", "^.+;(?<Title>.+);(?<Name>.+)$")]
    public class ComplexTestHeader
    {
        public string Title { get; set; }
        public string Name { get; set; }
    }

    [ParseFormat("02", "^.+;(?<Name>.+);(?<Description>.+);(?<More>.+)$")]
    [DebuggerDisplay("ComplexTestItems: {Name} - {Description} - {More}")]
    public class ComplexTestItems
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string More { get; set; }
    }
}

namespace StructuredFileParser.Tests.OtherNamespace
{
	[ParseRoot("^(?<Header>\\d{2});.+$")]
	public class OtherComplexTestRoot
	{
		public OtherComplexItem Item { get; set; }
	}

	[ParseFormat("01", "^.+;(?<Title>.+);(?<Name>.+)$")]
	public class OtherComplexItem
	{
		public string Title { get; set; }
		public string Name { get; set; }
	}

}
