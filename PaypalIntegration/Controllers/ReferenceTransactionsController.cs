//using PayPal.PayPalAPIInterfaceService;
//using PayPal.PayPalAPIInterfaceService.Model;
//using System.Collections.Generic;
//using System.Configuration;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Web;

using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using PaypalIntegration.Models;

namespace PaypalIntegration.Controllers
{
    public class ReferenceTransactionsController : Controller
    {
        /* Notes
         * Api Call for reference transaction
         * SetExpressCheckout -	Sets up a billing agreement and processes a purchase as part of the checkout process.	
         * CreateBillingAgreement - Sets up a billing agreement without a purchase.	
         * DoReferenceTransaction - Initiates a merchant pull payment. After a billing agreement is set up, this is the only call you need to make.	
         * BAUpdate	(Optional) - Obtains the buyer's most recent billing address when you set METHOD=BillAgreementUpdate. Also enables a buyer to cancel
                                 a billing agreement from your site. 
        */
        public ActionResult Index()
        {
            return View("ReferenceTransactions");
        }

        public void DoReferenceTranscations()
        {
            DoReferenceTransactionRequestType request = new DoReferenceTransactionRequestType();
            populateRequestObject(request);

            // Invoke the API
            DoReferenceTransactionReq wrapper = new DoReferenceTransactionReq();
            wrapper.DoReferenceTransactionRequest = request;


            Dictionary<string, string> configurationMap = Configuration.GetAcctAndConfig();

            // Create the PayPalAPIInterfaceServiceService service object to make the API call
            PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService(configurationMap);

            DoReferenceTransactionResponseType doReferenceTxnResponse = service.DoReferenceTransaction(wrapper);


            //setKeyResponseObjects(service, doReferenceTxnResponse);
        }


        //private void setKeyResponseObjects(PayPalAPIInterfaceServiceService service, DoReferenceTransactionResponseType response)
        //{
        //    HttpContext CurrContext = new System.Web.HttpContext(); ;
        //    CurrContext.Items.Add("Response_apiName", "DoReferenceTransaction");
        //    CurrContext.Items.Add("Response_redirectURL", null);
        //    CurrContext.Items.Add("Response_requestPayload", service.getLastRequest());
        //    CurrContext.Items.Add("Response_responsePayload", service.getLastResponse());

        //    Dictionary<string, string> responseParams = new Dictionary<string, string>();
        //    responseParams.Add("Correlation Id", response.CorrelationID);
        //    responseParams.Add("API Result", response.Ack.ToString());

        //    if (response.Ack.Equals(AckCodeType.FAILURE) ||
        //        (response.Errors != null && response.Errors.Count > 0))
        //    {
        //        CurrContext.Items.Add("Response_error", response.Errors);
        //    }
        //    else
        //    {
        //        CurrContext.Items.Add("Response_error", null);
        //        DoReferenceTransactionResponseDetailsType transactionDetails = response.DoReferenceTransactionResponseDetails;
        //        responseParams.Add("Transaction ID", transactionDetails.TransactionID);

        //        if (transactionDetails.PaymentInfo != null)
        //        {
        //            responseParams.Add("Payment status", transactionDetails.PaymentInfo.PaymentStatus.ToString());
        //        }

        //        if (transactionDetails.PaymentInfo != null)
        //        {
        //            responseParams.Add("Pending reason", transactionDetails.PaymentInfo.PendingReason.ToString());
        //        }
        //    }
        //    CurrContext.Items.Add("Response_keyResponseObject", responseParams);
        //    Server.Transfer("../APIResponse.aspx");
        //}

        private void populateRequestObject(DoReferenceTransactionRequestType request)
        {
            DoReferenceTransactionRequestDetailsType referenceTransactionDetails = new DoReferenceTransactionRequestDetailsType();
            request.DoReferenceTransactionRequestDetails = referenceTransactionDetails;

            referenceTransactionDetails.ReferenceID = "61K51535A9986391A";
            referenceTransactionDetails.PaymentAction = PaymentActionCodeType.SALE;


            PaymentDetailsType paymentDetails = new PaymentDetailsType();
            referenceTransactionDetails.PaymentDetails = paymentDetails;

            double orderTotal = 0.0;

            double itemTotal = 0.0;
            CurrencyCodeType currency = CurrencyCodeType.USD;

            PaymentDetailsItemType itemDetails = new PaymentDetailsItemType();

            itemDetails.Name = "ItemName";

            itemDetails.Amount = new BasicAmountType(currency, "10");

            itemDetails.Quantity = 2;

            itemDetails.ItemCategory = ItemCategoryType.DIGITAL;
            itemTotal += Convert.ToDouble(itemDetails.Amount.value) * itemDetails.Quantity.Value;

            paymentDetails.PaymentDetailsItem.Add(itemDetails);

            orderTotal += itemTotal;
            paymentDetails.ItemTotal = new BasicAmountType(currency, itemTotal.ToString());
            paymentDetails.OrderTotal = new BasicAmountType(currency, orderTotal.ToString());
        }
    }
}