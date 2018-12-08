//added a comment
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaypalIntegration.Controllers
{
    public class PaymentOptionsController : Controller
    {
        // GET: PaymentOptions
        public ActionResult Index(int quantity, string itemId, double itemCost, int shipmentTransactionId)
        {
            return View();
        }
    }
}