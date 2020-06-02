using AdapterDesignPattern.AkkaNetAdapter;
using Models;
using System.Collections.Generic;
using TechnicalIndicators;

namespace StrategyDesignPattern
{
    public class ActorModelStrategy : ICalculateTechnicalIndicatorStrategy
    {
        private IActorModelAdapter _adapter { get; set; }

        public ActorModelStrategy()
        {
            _adapter = new ActorModelAdapter();
        }

        public List<List<Signal>> ReceiveData(int countedTechnicalIndicatorsNumber)
        {
            return _adapter.ReceiveObtainedSignalsFromActorModelSystem();
        }

        public void SendData(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator)
        {
            _adapter.SendQuotesToCalculationOnCertainActor(quotes, parameters, indicator);
        }
    }
}
