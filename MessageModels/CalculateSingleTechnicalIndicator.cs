using Models;
using System.Collections.Generic;

namespace MessageModels
{
    public class CalculateSingleTechnicalIndicator
    {
        public List<Quote> Quotes { get; set; }
        public Parameters Parameters { get; set; }
    }
}
