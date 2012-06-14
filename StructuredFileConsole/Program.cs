using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using FlatFileConsole.Lib;
using FlatFileConsole.SimpleTest;
using FlatFileConsole.TwoDigitTest;
using FlatFileParser;

namespace FlatFileConsole
{
    static class Program
    {
        static void Main()
        {
            //Creates a parser
            var simpleRootParser = new FileParser<SimpleRoot>();

            Console.WriteLine("--- Simple ---");
            var simple = simpleRootParser.Parse(@"SimpleTest\Simple.txt");
            Console.WriteLine(XmlUtils.ToXml(simple));


            //Attempt to validate it
            ICollection<ValidationResult> validationResult = new Collection<ValidationResult>();
            var valid = Validator.TryValidateObject(simple, new ValidationContext(simple, null, null), validationResult, true);
            if (!valid)
            {
                Console.WriteLine("Item not valid!");
            }


            Console.WriteLine("--- Two Digit row type ---");
            var rootNode = new FileParser<TwoDigitList>().Parse(@"TwoDigitTest\TwoDigitListTestFile.txt");
            Console.WriteLine(XmlUtils.ToXml(rootNode));


            var rootNode2 = new FileParser<TwoDigit>().Parse(@"TwoDigitTest\TwoDigitTestFile.txt");
            Console.WriteLine(XmlUtils.ToXml(rootNode2));
            Console.WriteLine("Done!");
            Console.ReadLine();

        }
    }
}
