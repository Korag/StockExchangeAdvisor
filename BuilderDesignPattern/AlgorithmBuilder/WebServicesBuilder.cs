using StrategyDesignPattern;

namespace BuilderDesignPattern.AlgorithmBuilder
{
    public class WebServicesBuilder : IAlgorithmBuilder
    {
        private CalculateTechnicalIndicatorContext _strategyContext { get; set; }
        private ICalculateTechnicalIndicatorStrategy _strategy { get; set; }

        public void BuildAdditionalStrategyPatternParameters()
        {
            _strategy = new WebServicesStrategy();
        }

        public void BuildStrategyPattern()
        {
            _strategyContext = CalculateTechnicalIndicatorContext.GetInstance(_strategy);
        }

        public CalculateTechnicalIndicatorContext StrategyContext
        {
            get { return _strategyContext; }
        }
    }
}
