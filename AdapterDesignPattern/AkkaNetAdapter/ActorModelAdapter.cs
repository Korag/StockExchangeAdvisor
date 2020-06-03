using Actors;
using Akka.Actor;
using Akka.Configuration;
using MessageModels;
using Models;
using System;
using System.Collections.Generic;
using System.Threading;
using TechnicalIndicators;

namespace AdapterDesignPattern.AkkaNetAdapter
{
    public class ActorModelAdapter : IActorModelAdapter
    {
        private ActorSystem _system { get; set; }

        private Dictionary<List<Quote>, IActorRef> _queryActorsByQuotesSet { get; set; }
        private Dictionary<IActorRef, List<Quote>> _quotesByQueryActors { get; set; }
        private Dictionary<int, IActorRef> _queryActorsByThreadsIds { get; set; }

        private Object _padlock { get; set; }

        public ActorModelAdapter()
        {
            _padlock = new object();

            var config = ConfigurationFactory.ParseString(@"
                             akka.remote.dot-netty.tcp {
                             transport-class = ""Akka.Remote.Transport.DotNetty.DotNettyTransport, Akka.Remote""
                             transport-protocol = tcp
                             port = 8091
                             hostname = ""127.0.0.1""
                         }");

            _system = ActorSystem.Create("my-actor-server", config);

            _queryActorsByQuotesSet = new Dictionary<List<Quote>, IActorRef>();
            _quotesByQueryActors = new Dictionary<IActorRef, List<Quote>>();
            _queryActorsByThreadsIds = new Dictionary<int, IActorRef>();
        }

        public void SendQuotesToCalculationOnCertainActor(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator)
        {
            lock (_padlock)
            {
                _queryActorsByQuotesSet.TryGetValue(quotes, out IActorRef query);
                object message = new object();

                if (query == null)
                {
                    query = _system.ActorOf<QueryActor>("query" + _queryActorsByQuotesSet.Count);

                    _queryActorsByQuotesSet.Add(quotes, query);
                    _quotesByQueryActors.Add(query, quotes);

                    Thread thread = Thread.CurrentThread;
                    _queryActorsByThreadsIds.Add(thread.ManagedThreadId, query);
                }

                message = new CalculateSingleTechnicalIndicatorRequest()
                {
                    TechnicalIndicator = indicator,
                    Quotes = quotes,
                    Parameters = parameters,
                };

                query.Tell(message);
            }
        }

        public List<List<Signal>> ReceiveObtainedSignalsFromActorModelSystem()
        {
            lock (_padlock)
            {
                Thread thread = Thread.CurrentThread;
                _queryActorsByThreadsIds.TryGetValue(thread.ManagedThreadId, out IActorRef query);

                if (query == null)
                {
                    throw new EntryPointNotFoundException();
                }

                List<List<Signal>> obtainedSignals = new List<List<Signal>>();

                do
                {
                    obtainedSignals = query.Ask<List<List<Signal>>>("GetSignals").GetAwaiter().GetResult();

                } while (obtainedSignals.Count == 0);

                query.Tell("TerminateActorsRequest");

                _queryActorsByThreadsIds.Remove(thread.ManagedThreadId);

                _quotesByQueryActors.TryGetValue(query, out List<Quote> quotes);
                _quotesByQueryActors.Remove(query);
                _queryActorsByQuotesSet.Remove(quotes);

                return obtainedSignals;
                //_system.Dispose();
            }
        }
    }
}
