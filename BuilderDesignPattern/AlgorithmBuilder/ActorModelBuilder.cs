using StrategyDesignPattern;
using System;

namespace BuilderDesignPattern.AlgorithmBuilder
{
    public class ActorModelBuilder : IAlgorithmBuilder
    {
        private CalculateTechnicalIndicatorContext _strategyContext { get; set; }
        private ICalculateTechnicalIndicatorStrategy _strategy { get; set; }

        public void BuildAdditionalStrategyPatternParameters()
        {
            _strategy = new ActorModelStrategy();
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
