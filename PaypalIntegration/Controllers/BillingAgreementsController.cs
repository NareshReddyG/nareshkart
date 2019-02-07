using PayPal.Api;
using PaypalIntegration.Models;
using PaypalIntegration.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaypalIntegration.Controllers
{
    public class BillingAgreementsController : Controller
    {
        protected RequestFlow flow;
        public void Index()
        {
            flow = new RequestFlow();
            try
            {
                var apiContext = Utility.Configuration.GetAPIContext();

                string token = Request.Params["token"];
                if (string.IsNullOrEmpty(token))
                {
                    var returnUrl = this.CreateBillingAgreement(apiContext);
                    Response.Redirect(returnUrl);
                }
                else
                {
                    //this.ExecuteBillingAgreement(apiContext, token);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //protected RequestFlow flow;
        /// <summary>
        /// 
        /// </summary>
        private string CreateBillingAgreement(APIContext apiContext)
        {
            var returnUrl = string.Empty;
            try
            {
                // Before we can setup the billing agreement, we must first create a
                // billing plan that includes a redirect URL back to this test server.
                var plan = CreatePlanObject(System.Web.HttpContext.Current);
                var guid = Convert.ToString((new Random()).Next(100000));
                plan.merchant_preferences.return_url = Request.Url.ToString() + "?guid=" + guid;


                var createdPlan = plan.Create(apiContext);

                // Activate the plan
                var patchRequest = new PatchRequest()
                {
                    new Patch()
                    {
                        op = "replace",
                        path = "/",
                        value = new Plan() { state = "ACTIVE" }
                    }
                };


                createdPlan.Update(apiContext, patchRequest);

                // With the plan created and activated, we can now create the billing agreement.
                var payer = new Payer() { payment_method = "paypal" };
                var shippingAddress = new ShippingAddress()
                {
                    line1 = "111 First Street",
                    city = "Saratoga",
                    state = "CA",
                    postal_code = "95070",
                    country_code = "US"
                };

                var agreement = new Agreement()
                {
                    name = "T-Shirt of the Month Club",
                    description = "Agreement for T-Shirt of the Month Club",
                    start_date = "2019-02-19T00:37:04Z",//DateTimeOffset.Now.AddDays(2).ToString(),
                    payer = payer,

                    plan = new Plan() { id = createdPlan.id ,},// We can use the existing plan id 
                    shipping_address = shippingAddress
                };


                // Create the billing agreement.
                var createdAgreement = agreement.Create(apiContext);


                // Get the redirect URL to allow the user to be redirected to PayPal to accept the agreement.
                var links = createdAgreement.links.GetEnumerator();

                while (links.MoveNext())
                {
                    var link = links.Current;
                    if (link.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        this.flow.RecordRedirectUrl("Redirect to PayPal to approve billing agreement...", link.href);
                        returnUrl = link.href;
                    }
                }

                Session.Add(guid, createdAgreement.token);
                //Redirect()
                return returnUrl;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private void ExecuteBillingAgreement(APIContext apiContext, string token)
        {
            var agreement = new Agreement() { token = token };


            var executedAgreement = agreement.Execute(apiContext);


        }

        private static Currency GetCurrency(string value)
        {
            return new Currency() { value = value, currency = "USD" };
        }
        private static Plan CreatePlanObject(HttpContext httpContext)
        {
            // ### Create the Billing Plan
            // Both the trial and standard plans will use the same shipping
            // charge for this example, so for simplicity we'll create a
            // single object to use with both payment definitions.
            var shippingChargeModel = new ChargeModel()
            {
                type = "SHIPPING",
                amount = GetCurrency("9.99")
            };

            // Define the plan and attach the payment definitions and merchant preferences.
            // More Information: https://developer.paypal.com/webapps/developer/docs/api/#create-a-plan
            return new Plan
            {
                name = "T-Shirt of the Month Club Plan",
                description = "Monthly plan for getting the t-shirt of the month.",
                type = "fixed",
                // Define the merchant preferences.
                // More Information: https://developer.paypal.com/webapps/developer/docs/api/#merchantpreferences-object
                merchant_preferences = new MerchantPreferences()
                {
                    setup_fee = GetCurrency("1"),
                    return_url = httpContext.Request.Url.ToString(),
                    cancel_url = httpContext.Request.Url.ToString() + "?cancel",
                    auto_bill_amount = "YES",
                    initial_fail_amount_action = "CONTINUE",
                    max_fail_attempts = "0"
                },
                payment_definitions = new List<PaymentDefinition>
                {
                    // Define a trial plan that will only charge $9.99 for the first
                    // month. After that, the standard plan will take over for the
                    // remaining 11 months of the year.
                    new PaymentDefinition()
                    {
                        name = "Trial Plan",
                        type = "TRIAL",
                        frequency = "MONTH",
                        frequency_interval = "1",
                        amount = GetCurrency("9.99"),
                        cycles = "1",
                        charge_models = new List<ChargeModel>
                        {
                            new ChargeModel()
                            {
                                type = "TAX",
                                amount = GetCurrency("1.65")
                            },
                            shippingChargeModel
                        }
                    },
                    // Define the standard payment plan. It will represent a monthly
                    // plan for $19.99 USD that charges once month for 11 months.
                    new PaymentDefinition
                    {
                        name = "Standard Plan",
                        type = "REGULAR",
                        frequency = "MONTH",
                        frequency_interval = "1",
                        amount = GetCurrency("19.99"),
                        // > NOTE: For `IFNINITE` type plans, `cycles` should be 0 for a `REGULAR` `PaymentDefinition` object.
                        cycles = "11",
                        charge_models = new List<ChargeModel>
                        {
                            new ChargeModel
                            {
                                type = "TAX",
                                amount = GetCurrency("2.47")
                            },
                            shippingChargeModel
                        }
                    }
                }
            };
        }
    }
}