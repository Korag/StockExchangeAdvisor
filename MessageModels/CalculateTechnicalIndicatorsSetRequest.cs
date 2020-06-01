using System;
using System.Collections.Generic;
using TechnicalIndicators;

namespace MessageModels
{
    public class CalculateTechnicalIndicatorsSetRequest
    {
        public List<TechnicalIndicator> TechnicalIndicators { get; set; }
        public CalculateSingleTechnicalIndicator IndicatorCalculationData { get; set; }
    }
}
