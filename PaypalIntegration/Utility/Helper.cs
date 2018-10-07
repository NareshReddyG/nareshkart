using System.Collections.Generic;
using System.Web.Mvc;

namespace PaypalIntegration.Utility
{
    public static class Helper
    {
        public static IEnumerable<SelectListItem> GetCountryList()
        {
            var items = new List<SelectListItem>();
            items.Insert(0, new SelectListItem() { Text = "--Select--", Value = "" });
            items.Insert(1, new SelectListItem() { Text = "India", Value = "1" });
            items.Insert(2, new SelectListItem() { Text = "United States of America (USA)", Value = "2" });
            return items;
        }
    }
}