using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using Utility;
using WebServiceAPI.Models;

namespace WebServiceAPI.Controllers
{
    [ApiController]
    public class TechnicalIndicatorsController : ControllerBase
    {
        public string GeneratedSignalsURL { get; set; }

        public TechnicalIndicatorsController()
        {
            GeneratedSignalsURL = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\WebServiceAPI\\GeneratedSignals\\"));
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

        [Route("api/CalculateTechnicalIndicator")]
        [HttpPost]
        public IActionResult CalculateTechnicalIndicator(Utility.IndicatorCalculationElementsWIndicatorType data)
        {
            try
            {
                List<Signal> obtainedSignals = data.TechnicalIndicator.GetSignals(data.Quotes, data.Parameters);
                string jsonString = JsonSerializer.CollectionOfSignalsToJsonString(obtainedSignals);

                int filesCount = FileHelper.CountFilesInDirectory(GeneratedSignalsURL);
                int newFileId = (filesCount == 0 ? 0 : filesCount + 1);
                string fileName = $"signals_{newFileId}.json";
                FileHelper.SaveJsonFile(GeneratedSignalsURL + fileName, jsonString);

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

                if (String.IsNullOrWhiteSpace(jsonString))
                {
                    return StatusCode(404);
                }

                return StatusCode(200, jsonString);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}