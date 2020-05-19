using System;
using System.Net.Http;
using Utility;
using WebServicesModels;

namespace WebService
{
    public class WebServiceCalculateIndicator : IWebServiceCalculateIndicator
    {
        private HttpClient _client { get; set; }
        private string _serverIpAddress = "40.115.121.134";

        public WebServiceCalculateIndicator()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_serverIpAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new
              System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public int CalculateTechnicalIndicator(IndicatorCalculationElementsWIndicatorType data)
        {
            string jsonString = JsonSerializer.CollectionOfIndicatorCalculationElementsWIndicatorTypeToJsonString(data);
            int idOfCalculation = 0;

            try
            {
                HttpResponseMessage response = _client.PostAsync("api/CalculateTechnicalIndicator", new StringContent(jsonString,
                                         System.Text.Encoding.UTF8, "application/json")).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    idOfCalculation = JsonSerializer.JsonStringToInt(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return idOfCalculation;
        }

        public int GetAmountOfGeneratedSignalsFiles()
        {
            HttpResponseMessage response;
            string responseString;
            int filesCount = 0;

            try
            {
                response = _client.GetAsync("api/GetAmountOfGeneratedSignalsFiles").Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    responseString = response.Content.ReadAsStringAsync().Result;
                    filesCount = JsonSerializer.JsonStringToInt(responseString);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return filesCount;
        }

        public string GetObtainedSignals(int id)
        {
            HttpResponseMessage response;
            string responseString;

            try
            {
                response = _client.GetAsync("api/GetAmountOfGeneratedSignalsFiles").Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    responseString = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return responseString;
        }
    }
}
