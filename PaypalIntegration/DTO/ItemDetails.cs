using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaypalIntegration.DTO
{
    public class ItemDetails
    {
        //Item Details
        public int Quantity { get; set; }
        public string ItemId { get; set; }
        public double ItemCost { get; set; }
    }
}