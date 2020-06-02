using Models;
using System;
using System.Collections.Generic;
using System.Text;
using TechnicalIndicators;

namespace AdapterDesignPattern.AkkaNetAdapter
{
    public interface IActorModelAdapter
    {
        void SendQuotesToCalculationOnCertainActor(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator);
        List<List<Signal>> ReceiveObtainedSignalsFromActorModelSystem();
    }
}
