using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using Utility;
using WebServicesModels;

namespace WebService
{
    public class WebServiceCalculateIndicator : IWebServiceCalculateIndicator
    {
        private HttpClient _client { get; set; }
        //private string _serverIpAddress = "40.115.121.134";
        private string _serverIpAddress = "https://localhost:44361";
        
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
            string jsonString = Utility.JsonSerializer.ConvertObjectToJsonString<IndicatorCalculationElementsWIndicatorType>(data);
            //string jsonString = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            //{
            //    PreserveReferencesHandling = PreserveReferencesHandling.None,
            //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //    Formatting = Formatting.Indented,
            //    TypeNameHandling = TypeNameHandling.Auto
            //});

            //problem z apką na serwerze
            //reszta OK

            int idOfCalculation = 0;

            try
            {
                HttpContent payload = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync("api/CalculateTechnicalIndicator", payload).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    idOfCalculation = Utility.JsonSerializer.JsonStringToObjectType<int>(response.Content.ReadAsStringAsync().Result);
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
                    filesCount = Utility.JsonSerializer.JsonStringToObjectType<int>(responseString);
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
                response = _client.GetAsync($"api/GetObtainedSignals/{id}").Result;

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
