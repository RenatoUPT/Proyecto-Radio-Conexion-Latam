using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RadioConexionLatam.Models
{
    public class CulqiService
    {
        private readonly string publicKey;
        private readonly string secretKey;

        public CulqiService(string publicKey, string secretKey)
        {
            this.publicKey = publicKey;
            this.secretKey = secretKey;
        }

        public string CreateCharge(string tokenId, int amount, string currency, string description)
        {
            var client = new RestClient("https://api.culqi.com/v2/charges");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + this.secretKey);
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

        public string CreateToken(string cardNumber, int expMonth, int expYear, string cvv)
        {
            var client = new RestClient("https://secure.culqi.com/v2/tokens");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + this.publicKey);
            request.AddHeader("Content-Type", "application/json");

            var tokenData = new
            {
                card_number = cardNumber,
                cvv = cvv,
                expiration_month = expMonth,
                expiration_year = expYear,
                email = "fv2020066896@virtual.upt.pe"
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
    }
        
}