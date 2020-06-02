using Models;
using System.Collections.Generic;
using TechnicalIndicators;

namespace MessageModels
{
    public class CalculateSingleTechnicalIndicatorRequest
    {
        public TechnicalIndicator TechnicalIndicator { get; set; }
        public List<Quote> Quotes { get; set; }
        public Parameters Parameters { get; set; }
    }
}
