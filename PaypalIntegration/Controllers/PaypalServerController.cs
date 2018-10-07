using PayPal.Api;
using PaypalIntegration.DataAccess.Repository;
using PaypalIntegration.DTO;
using PaypalIntegration.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PaypalIntegration.Controllers
{
    public class PaypalServerController : Controller
    {
        // GET: PaypalServer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentWithPayPal(double amount, int shipmentTransactionId)
        {
            // Get ShippingDetails By TransactionId
            IShippingDetailsRepository shippingDetailsRepository = new ShippingDetailsRepository();
            var shippingDetails = shippingDetailsRepository.GetShippingDetailsById(shipmentTransactionId);
            //getting the apiContext as earlier
            APIContext apiContext = PaypalConfiguation.GetAPIContext();
            string transactionGeneratedId = string.Empty;
            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/PaypalServer/PaymentRedirect?";

                    var guid = Convert.ToString((new Random()).Next(100000));

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, amount);

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);
                    transactionGeneratedId = createdPayment.id;
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters
                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                    transactionGeneratedId = executedPayment.id;

                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            // Success view
            return RedirectToAction("Index", "TranactionSuccess", new { transactionId = transactionGeneratedId });
        }

        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, double amount)
        {
            Item item = new Item();
            item.name = new Guid().ToString();
            item.currency = "USD";
            item.price = amount.ToString();
            item.quantity = "1";
            item.sku = "sku";

            //Now make a List of Item and add the above item to itc
            //you can create as many items as you want and add to this list
            List<Item> itms = new List<Item>();
            itms.Add(item);
            ItemList itemList = new ItemList();
            itemList.items = itms;
            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&isFrom=cancel",
                return_url = redirectUrl + "&isFrom=returnurl"
            };

            // Specify details of your payment amount.
            Details details = new Details();
            details.shipping = "0";
            details.subtotal = amount.ToString();
            details.tax = "0";

            Amount amnt = new Amount();
            amnt.currency = "USD";
            // Total = shipping tax + subtotal.
            amnt.total = amount.ToString();
            amnt.details = details;

            // Now make a trasaction object and assign the Amount object
            Transaction transaction = new Transaction();
            transaction.amount = amnt;
            transaction.description = "Assignment in serverside program for paypal interview";
            transaction.item_list = itemList;
            transaction.invoice_number = "TestInvoiceNumber";
            List<Transaction> transactionList = new List<Transaction>();
            transactionList.Add(transaction);
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);

        }

        public ActionResult PaymentRedirect(string guid, string isFrom, string token)
        {
            var paymentTransactionId = Convert.ToString(Session[guid]);

            return RedirectToAction("Index", "TranactionSuccess", new { transactionId = paymentTransactionId });
        }

    }
}