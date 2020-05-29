using StrategyDesignPattern;
using Utility;

namespace BuilderDesignPattern.AlgorithmBuilder
{
    public class RabbitMQBuilder : IAlgorithmBuilder
    {
        private CalculateTechnicalIndicatorContext _strategyContext { get; set; }
        private ICalculateTechnicalIndicatorStrategy _strategy { get; set; }

        private string _exchange = "SignalsExchange";
        private string _queueReceiveFrom = "ObtainedSignals";

        public void BuildStrategyPatternParameters()
        {
            _strategy = new RabbitMQStrategy(_exchange, _queueReceiveFrom);
            ProcessHandler.RunRabbitMQConsumersProcesses();
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
