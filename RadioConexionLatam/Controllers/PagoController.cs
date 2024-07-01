using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RadioConexionLatam.Controllers
{
    public class PagoController : Controller
    {
        private readonly string publicKey = "pk_test_UTCQSGcXW8bCyU59";
        private readonly string secretKey = "sk_test_wJ7jU1ydtz9npsmc";

        // GET: Pago
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProcesarPago(string cardNumber, int expMonth, int expYear, string cvv)
        {
            try
            {
                // Crear token
                string tokenId = CreateToken(cardNumber, expMonth, expYear, cvv);

                // Crear cargo
                string chargeResponse = CreateCharge(tokenId, 1000, "PEN", "Descripción del cargo");

                // Manejar la respuesta
                ViewBag.Message = "Pago realizado con éxito: " + chargeResponse;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
            }

            return View("Index");
        }

        private string CreateToken(string cardNumber, int expMonth, int expYear, string cvv)
        {
            var client = new RestClient("https://secure.culqi.com/v2/tokens");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {this.publicKey}");
            request.AddHeader("Content-Type", "application/json");

            var tokenData = new
            {
                card_number = cardNumber,
                cvv = cvv,
                expiration_month = expMonth,
                expiration_year = expYear,
                email = "rc2020066918@virtual.upt.pe"
            };

            request.AddJsonBody(tokenData);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return result.id;
            }
            else
            {
                throw new Exception("Error al crear el token: " + response.Content);
            }
        }

        private string CreateCharge(string tokenId, int amount, string currency, string description)
        {
            var client = new RestClient("https://api.culqi.com/v2/charges");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {this.secretKey}");
            request.AddHeader("Content-Type", "application/json");

            var chargeData = new
            {
                amount = amount,
                currency_code = currency,
                email = "fv2020066896@virtual.upt.pe",
                source_id = tokenId,
                description = description
            };

            request.AddJsonBody(chargeData);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                throw new Exception("Error al crear el cargo: " + response.Content);
            }
        }
    }
}