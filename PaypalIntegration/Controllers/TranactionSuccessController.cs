using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaypalIntegration.Controllers
{
    public class TranactionSuccessController : Controller
    {
        // GET: TranactionSuccess
        public ActionResult Index(string transactionId)
        {
            ViewBag.TransactionId = transactionId;
            return View();
        }
    }
}