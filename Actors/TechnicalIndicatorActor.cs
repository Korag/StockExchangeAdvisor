using Akka.Actor;
using MessageModels;
using Models;
using System;
using System.Collections.Generic;
using TechnicalIndicators;

namespace Actors
{
    public class TechnicalIndicatorActor : UntypedActor
    {
        private TechnicalIndicator _indicator;
        private List<Signal> _obtainedSignals = new List<Signal>();

        public TechnicalIndicatorActor(TechnicalIndicator indicator)
        {
            _indicator = indicator;
        }

        protected override void PreStart()
        {
            Console.WriteLine(_indicator.GetType() + "Actor started.");
        }

        protected override void PostStop()
        {
            Console.WriteLine(_indicator.GetType() + "Actor stopped.");
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case CalculateSingleTechnicalIndicatorRequest req:

                    try
                    {
                        _obtainedSignals = _indicator.GetSignals(req.Quotes, req.Parameters);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(_indicator.GetType() + "Actor terminated due to exception.");

                        Sender.Tell("ErrorDuringCalculation");
                        Context.Stop(Self);
                    }

                    Sender.Tell(new SaveObtainedSignalsRequest(_obtainedSignals));
                    break;

                case "GetObtainedSignalsRequest":
                    Sender.Tell(new SaveObtainedSignalsRequest(_obtainedSignals));
                    break;
            }
        }

        public static Props Props(TechnicalIndicator indicator)
        {
            return Akka.Actor.Props.Create(() => new TechnicalIndicatorActor(indicator));
        }
    }
}
