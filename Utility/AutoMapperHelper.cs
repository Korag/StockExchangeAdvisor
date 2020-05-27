using AutoMapper;
using DecoratorDesignPattern;
using Models;
using StateAndDecoratorDesignPattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class AutoMapperHelper
    {
        private static IMapper _mapper = null;

        public static IMapper GetInstance()
        {
            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                });

                _mapper = new Mapper(config);
            }

            return _mapper;
        }

        public static List<SignalModelContext> MapQuotesAndSignalsToSignalModelContext(List<Quote> quotes, List<Signal> signals)
        {
            List<SignalModelContext> signalsContext = new List<SignalModelContext>();

            for (int i = 0; i < signals.Count(); i++)
            {
                Quote matchingQuote = quotes.Where(z => z.Date == signals[i].Date).FirstOrDefault();

                SignalModelContext signalContext = _mapper.Map<SignalModelContext>(matchingQuote);
                signalContext.PartialSignals.Add(signals[i].Value);

                signalsContext.Add(signalContext);
            }

            return signalsContext;
        }

        public static List<SignalModelContext> MapQuotesAndSignalsToSignalModelContext(List<Quote> quotes, List<List<Signal>> signals)
        {
            List<SignalModelContext> signalsContext = new List<SignalModelContext>();
            
            for (int i = 0; i < signals.Count(); i++)
            {
                for (int j = 0; j < signals[i].Count(); j++)
                {
                    Quote matchingQuote = quotes.Where(z => z.Date == signals[i][j].Date).FirstOrDefault();
                    SignalModelContext signalContext = signalsContext.Where(z => z.Date == matchingQuote.Date).FirstOrDefault();

                    if (signalContext == null)
                    {
                        signalContext = _mapper.Map<SignalModelContext>(matchingQuote);
                        signalContext.PartialSignals.Add(signals[i][j].Value);
                        signalsContext.Add(signalContext);
                    }
                    else
                    {
                        signalsContext.Where(z => z.Date == matchingQuote.Date).FirstOrDefault().PartialSignals.Add(signals[i][j].Value);
                    }
                }
            }

            List<string> signalsDates = signalsContext.Select(z => z.Date).ToList();
            List<Quote> quotesWithoutAnyGeneratedSignal = quotes.Where(z => !signalsDates.Contains(z.Date)).ToList();

            foreach (var quote in quotesWithoutAnyGeneratedSignal)
            {
                SignalModelContext signalContext = _mapper.Map<SignalModelContext>(quote);
                signalsContext.Add(signalContext);
            }

            return signalsContext;
        }

        public static DecoratorConcreteComponent MapQuotesAndSignalsToDecoratorObject(SignalModelContext quoteWSignals)
        {
            return _mapper.Map<DecoratorConcreteComponent>(quoteWSignals);
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Quote, SignalModelContext>();
            CreateMap<List<Quote>, List<SignalModelContext>>();
            CreateMap<SignalModelContext, DecoratorConcreteComponent>();
        }
    }
}
