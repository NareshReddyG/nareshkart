using PaypalIntegration.DataAccess.Repository;
using PaypalIntegration.DTO;
using PaypalIntegration.Utility;
using System.Web.Mvc;

namespace PaypalIntegration.Controllers
{
    public class ShippingDetailsController : Controller
    {
        // GET: ShippingDetails
        public ActionResult Index()
        {
            var personDetails = new ShippingDetailsDTO();
            return View(personDetails);
        }

        public ActionResult GetShippingDetails(int quantity, string itemId, double itemCost)
        {
            var itemDetails = new ItemDetails
            {
                ItemCost = itemCost,
                ItemId = itemId,
                Quantity = quantity
            };
            // Get person details
            var personDetails = new ShippingDetailsDTO();
            personDetails.Country = 1;
            personDetails.CountryList = Helper.GetCountryList();
            personDetails.ItemDetails = itemDetails;
            return View("~/Views/ShippingDetails/Index.cshtml", personDetails);
        }
        public ActionResult SaveShippingDetails(ShippingDetailsDTO shippingDetails)
        {
            if (!ModelState.IsValid)
            {
                shippingDetails.CountryList = Helper.GetCountryList();
                return View("~/Views/ShippingDetails/Index.cshtml", shippingDetails);
            }
            IShippingDetailsRepository shippingDetailsRepository = new ShippingDetailsRepository();
            var shippingTransactionId = shippingDetailsRepository.SaveShippingDetails(shippingDetails);
            return RedirectToAction("Index", "PaymentOptions", new
            {
                quantity = shippingDetails.ItemDetails?.Quantity,
                itemId = shippingDetails.ItemDetails?.ItemId,
                itemCost = shippingDetails.ItemDetails?.ItemCost,
                shipmentTransactionId = shippingTransactionId
            });
        }
    }
}