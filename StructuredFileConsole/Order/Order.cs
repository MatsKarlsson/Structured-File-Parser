using System.Collections.Generic;
using FlatFileParser;

namespace FlatFileConsole.Order
{

    /// <summary>
    /// Order semicolon separarated, where first word is type of item
    /// </summary>
    [ParseRoot(@"^(?<Trigger>.+?);.+$")]
    public class OrderFile
    {
        public List<Order> Orders{ get; set; }
    }

    [ParseFormat("Order", "^.+;(?<DeliveryDate>.+)$")]
    public class Order
    {
        public string DeliveryDate { get; set; }
        public List<Item> Items { get; set; }
    }

    [ParseFormat("Item", @"^.+?;(?<Name>.+?);(?<Amount>.+?);(?<Price>.+)$")]
    public class Item
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}

