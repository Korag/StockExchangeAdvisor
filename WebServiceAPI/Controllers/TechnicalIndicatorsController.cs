using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using Utility;
using WebServicesModels;

namespace WebServiceAPI.Controllers
{
    [ApiController]
    public class TechnicalIndicatorsController : ControllerBase
    {
        public string GeneratedSignalsURL { get; set; }
        public static Object _padlock = new object();

        public TechnicalIndicatorsController()
        {
            //Local URL
            //GeneratedSignalsURL = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\WebServiceAPI\\GeneratedSignals\\"));

            //Azure URL
            GeneratedSignalsURL = "/var/www/html/StockExchangeAdvisorAPI/GeneratedSignals/";
        }

        [Route("api/Hello")]
        [HttpGet]
        public string Hello()
        {
            return "Hello world!";
        }

        [Route("api/GetAmountOfGeneratedSignalsFiles")]
        [HttpGet]
        public IActionResult GetAmountOfGeneratedSignalsFiles()
        {
            return StatusCode(200, FileHelper.CountFilesInDirectory(GeneratedSignalsURL));
        }

        [Route("api/GetSignalsURL")]
        [HttpGet]
        public IActionResult GetSignalsURL()
        {
            return StatusCode(200, GeneratedSignalsURL);
        }

        [Route("api/CalculateTechnicalIndicator")]
        [HttpPost]
        public IActionResult CalculateTechnicalIndicator(IndicatorCalculationElementsWIndicatorType data)
        {
            try
            {
                List<Signal> obtainedSignals = data.TechnicalIndicator.GetSignals(data.Quotes, data.Parameters);
                string jsonString = JsonSerializer.ConvertCollectionOfObjectsToJsonString<Signal>(obtainedSignals);

                int newFileId;

                lock (_padlock)
                {
                    int filesCount = FileHelper.CountFilesInDirectory(GeneratedSignalsURL);
                    newFileId = (filesCount == 0 ? 0 : filesCount);
                    string fileName = $"signals_{newFileId}.json";
                    FileHelper.SaveJsonFile(GeneratedSignalsURL + fileName, jsonString);
                }

                return StatusCode(201, newFileId);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [Route("api/GetObtainedSignals/{id}")]
        [HttpGet]
        public IActionResult GetObtainedSignals(int id)
        {
            try
            {
                string jsonString = FileHelper.ReadFile(GeneratedSignalsURL + $"signals_{id}.json");
                List<Signal> obtainedSignals = JsonSerializer.JsonStringToCollectionOfObjectsTypes<Signal>(jsonString);

                if (String.IsNullOrWhiteSpace(jsonString))
                {
                    return StatusCode(404);
                }

                return StatusCode(200, obtainedSignals);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}