using System.Configuration;
using System.Web.Mvc;

namespace PaypalIntegration.Controllers
{
    public class ClientSidePaymentController : Controller
    {
        // GET: PaymentCheckout
        public ActionResult Index(int amount)
        {
            ViewBag.Amount = amount;
            ViewBag.ClientId = ConfigurationManager.AppSettings["paypal:clientid"];
            return View();
        }
    }
}