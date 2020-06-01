using Akka.Actor;
using MessageModels;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using TechnicalIndicators;

namespace Actors
{
    public class QueryActor : UntypedActor
    {
        private Dictionary<string, IActorRef> _technicalIndicatorTypeToActor = new Dictionary<string, IActorRef>();

        private List<List<Signal>> _obtainedSignals = new List<List<Signal>>();
        private int _actorErrorsCount;

        private bool _jobFinished = false;

        public QueryActor()
        {

        }

        protected override void PreStart()
        {
            Console.WriteLine("DelegateActor started.");
        }

        protected override void PostStop()
        {
            {
                Console.WriteLine("DelegateActor stopped.");
            }
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case CalculateTechnicalIndicatorsSetRequest req:

                    foreach (var technicalIndicator in req.TechnicalIndicators)
                    {
                        var tiActor = Context.ActorOf(TechnicalIndicatorActor.Props(technicalIndicator));
                        _technicalIndicatorTypeToActor.Add(technicalIndicator.GetType().ToString(), tiActor);
                        tiActor.Tell(req.IndicatorCalculationData);
                    }
                    break;

                case SaveObtainedSignalsRequest req:

                    _obtainedSignals.Add(req.Signals);

                    if (_obtainedSignals.Count + _actorErrorsCount == _technicalIndicatorTypeToActor.Count)
                    {
                        _jobFinished = true;
                    }
                    break;

                case "ErrorDuringCalculation":
                    _actorErrorsCount++;
                    break;

                case "TerminateActorsRequest":
                    Context.Stop(Self);
                    break;
            }
        }

        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new QueryActor());
        }
    }
}
