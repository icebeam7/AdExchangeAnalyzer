using System;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using Newtonsoft.Json;

using AdExchangeAnalyzer.Models;
using AdExchangeAnalyzer.Helpers;

namespace AdExchangeAnalyzer.Services
{
    public static class AnomalyDetectorService
    {
        private static readonly HttpClient client = CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Constants.Endpoint);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constants.SubscriptionKey);
            return client;
        }

        public async static Task<DataResult> AnalyzeData(DataRequest dataRequest)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                    | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var data = JsonConvert.SerializeObject(dataRequest);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Constants.DetectAnomaliesServiceURL, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var priceResult = JsonConvert.DeserializeObject<DataResult>(jsonResult);
                    return priceResult;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
    }
}
