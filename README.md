
# Structured-File-Parser
This library is created to easily parse structured files.

## What is the purpose of the library

To parse structured files and create instances of POCO's
It's like XmlSerializer, but for structured text files

## Structured files?

Files in a specific format that could be grouped into columns or similar.
It could be CSV-files, semicolon delimitered, tab, fixed widtdh.

It could be a simple list of items or hierarhical files containg items and sub items.

### What could it do?

It could turn a order file containing the following rows:

* Order;123;2012-01-01
* Item;Thing;2;3.20
* Item;Bucket;2;2.50
* Item;Stuff;2;2.00
* Item;Thing;2;2.10
* Order;124;2012-02-02
* Item;More Stuff;12;12.40
* Item;Plenty of Buckets;22;100

Into a OrderInstance with a list of two orders containing four and two items
    
    public class OrderFile
    {
        public List<Order> Orders{ get; set; }
    }

    public class Order
    {
        public string DeliveryDate { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }


There are two main modes: 
Parse a file and get a list of objects back or get a hierarchical class 

### How does it know how to fill the objects?
You decorate your classes with a simple attribute.
The main instance is decorated with a `ParseRoot` attribute with a regular expression describing how to extract the sub-objects (for example a order row or a order item).

The objects that should be instanciated have a `ParseFormat` attribute containing the row identifier (for example "Item") and a regular expression describing how and what to parse from the row in the text file (for example: `^.+?;(?<Name>.+?);(?<Amount>.+?);(?<Price>.+)$`

### What format does it support?

The files could be fixed width, delimitered.

To use it you create simple POCO classes containing the structure of the text file and decorate them with attributes containing regular expressions.

## Do you have any examples?

Yes the project `StructuredFileConsole` contains a couple of examples.

## License

[MIT License](https://github.com/MatsKarlsson/Structured-File-Parser/blob/license.md)