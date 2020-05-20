using Models;
using System.Collections.Generic;
using TechnicalIndicators;

namespace WebServicesModels
{
    public class IndicatorCalculationElementsWIndicatorType
    {
        public Parameters Parameters { get; set; }
        public TechnicalIndicator TechnicalIndicator { get; set; }
        public List<Quote> Quotes { get; set; }
    }
}
