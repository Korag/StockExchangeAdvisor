using Models;
using System.Collections.Generic;
using TechnicalIndicators;

namespace WebService
{
    public class IndicatorCalculationElementsWIndicatorType
    {
        public List<Quote> Quotes { get; set; }
        public Parameters Parameters { get; set; }
        public TechnicalIndicator TechnicalIndicator { get; set; }
    }
}
