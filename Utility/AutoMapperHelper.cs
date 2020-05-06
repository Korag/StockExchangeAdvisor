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

        #region Legacy
        //public static QuoteWithSignal MapQuoteToQuoteWithSignal(Quote quote, QuoteWithSignal quoteWithSignal = null)
        //{
        //    if (quoteWithSignal == null)
        //    {
        //        quoteWithSignal = _mapper.Map<QuoteWithSignal>(quote);
        //    }
        //    else
        //    {
        //        quoteWithSignal = _mapper.Map<Quote, QuoteWithSignal>(quote, quoteWithSignal);
        //    }

        //    return quoteWithSignal;
        //}

        //public static QuoteWithSignal MapSignalToQuoteWithSignal(Signal signal, QuoteWithSignal quoteWithSignal = null)
        //{
        //    if (quoteWithSignal == null)
        //    {
        //        quoteWithSignal = _mapper.Map<QuoteWithSignal>(signal);
        //    }
        //    else
        //    {
        //        quoteWithSignal = _mapper.Map<Signal, QuoteWithSignal>(signal, quoteWithSignal);
        //    }

        //    return quoteWithSignal;
        //}

        //public static List<QuoteWithSignal> MapQuoteListToQuoteWithSignalList(List<Quote> quotes, List<QuoteWithSignal> quotesWithSignal = null)
        //{
        //    if (quotesWithSignal == null)
        //    {
        //        quotesWithSignal = _mapper.Map<List<QuoteWithSignal>>(quotes);
        //    }
        //    else
        //    {
        //        quotesWithSignal = _mapper.Map<List<Quote>, List<QuoteWithSignal>>(quotes, quotesWithSignal);
        //    }

        //    return quotesWithSignal;
        //}

        //public static List<QuoteWithSignal> MapSignalListToQuoteWithSignalList(List<Signal> signal, List<QuoteWithSignal> quotesWithSignal = null)
        //{
        //    if (quotesWithSignal == null)
        //    {
        //        quotesWithSignal = _mapper.Map<List<QuoteWithSignal>>(signal);
        //    }
        //    else
        //    {
        //        quotesWithSignal = _mapper.Map<List<Signal>, List<QuoteWithSignal>>(signal, quotesWithSignal);
        //    }

        //    return quotesWithSignal;
        //}

        //public static Quote MapQuoteWithSignalToQuote(QuoteWithSignal quoteWithSignal)
        //{
        //    return _mapper.Map<Quote>(quoteWithSignal);
        //}

        //public static Signal MapQuoteWithSignalToSignal(QuoteWithSignal quoteWithSignal)
        //{
        //    return _mapper.Map<Signal>(quoteWithSignal);
        //}

        //public static List<Quote> MapQuoteWithSignalListToQuoteList(List<QuoteWithSignal> quoteWithSignal)
        //{
        //    List<Quote> quotes = _mapper.Map<List<Quote>>(quoteWithSignal);
        //    return quotes;
        //}

        //public static List<Signal> MapQuoteWithSignalListToSignalList(List<QuoteWithSignal> quoteWithSignal)
        //{
        //    List<Signal> signals = _mapper.Map<List<Signal>>(quoteWithSignal);
        //    return signals;
        //}
        #endregion

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
            #region Legacy
            //CreateMap<Quote, QuoteWithSignal>();
            //CreateMap<Signal, QuoteWithSignal>();
            //CreateMap<List<Quote>, List<QuoteWithSignal>>();
            //CreateMap<List<Signal>, List<QuoteWithSignal>>();

            //CreateMap<QuoteWithSignal, Quote>();
            //CreateMap<QuoteWithSignal, Signal>();
            //CreateMap<List<QuoteWithSignal>, List<Quote>>();
            //CreateMap<List<QuoteWithSignal>, List<Signal>>();
            #endregion

            CreateMap<Quote, SignalModelContext>();
            CreateMap<List<Quote>, List<SignalModelContext>>();
            CreateMap<SignalModelContext, DecoratorConcreteComponent>();
        }
    }
}
