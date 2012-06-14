using System.ComponentModel.DataAnnotations;
using FlatFileParser;

namespace FlatFileConsole.SimpleTest
{

    /// <summary>
    /// Root node, anything up to first semicolon is to extract row type
    /// </summary>
    [ParseRoot("^(?<RowType>.+?);.+$")]
    public class SimpleRoot
    {
        public First SimpleChildOfRoot { get; set; }

        public AdditionalInfo Info { get; set; }

        public AdditionalInfo InfoRequired { get; set; }
    }

    [ParseFormat("First", "^.+;(?<Title>.+);(?<Description>.+)$")]
    public class First
    {
        [StringLength(4, ErrorMessage = "wtf! should be only 4 long")]
        public string Title { get; set; }

        public string Description{ get; set; }

        public Child ChildNode { get; set; }
    }

    [ParseFormat("Child", "^.+;(?<ChildTitle>.+);(?<ChildDescription>.+)$")]
    public class Child
    {
        public string ChildTitle { get; set; }

        public string ChildDescription { get; set; }
        
        public AdditionalInfo Info { get; set; }
    }

    [ParseFormat("AdditionalInfo", "^.+;(?<Info>.+);(?<SomeOptionalNumber>.*)$")]
    public class AdditionalInfo
    {
        public string Info { get; set; }

        public int? SomeOptionalNumber { get; set; }
    }


}
