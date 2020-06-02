using Akka.Actor;
using MessageModels;
using Models;
using System;
using System.Collections.Generic;
using TechnicalIndicators;

namespace Actors
{
    public class QueryActor : UntypedActor
    {
        private Dictionary<TechnicalIndicator, IActorRef> _technicalIndicatorTypeToActor = new Dictionary<TechnicalIndicator, IActorRef>();

        private List<List<Signal>> _obtainedSignals = new List<List<Signal>>();

        private int _reqAmount;
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
            Console.WriteLine("DelegateActor stopped.");
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case CalculateSingleTechnicalIndicatorRequest req:

                    _reqAmount++;
                    _technicalIndicatorTypeToActor.TryGetValue(req.TechnicalIndicator, out IActorRef tiActor);

                    if (tiActor == null)
                    {
                        tiActor = Context.ActorOf(TechnicalIndicatorActor.Props(req.TechnicalIndicator));
                        _technicalIndicatorTypeToActor.Add(req.TechnicalIndicator, tiActor);
                    }

                    tiActor.Tell(req);
                    break;

                case SaveObtainedSignalsRequest req:

                    _obtainedSignals.Add(req.Signals);

                    if (_obtainedSignals.Count + _actorErrorsCount == _reqAmount)
                    {
                        _jobFinished = true;
                    }
                    break;

                case "GetSignals":
                  
                    if (!_jobFinished)
                    {
                        Sender.Tell(new List<List<Signal>>());
                    }

                    Sender.Tell(_obtainedSignals);
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
