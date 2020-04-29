using AutoMapper;
using Models;
using System.Collections.Generic;

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

        public static QuoteWithSignal MapQuoteToQuoteWithSignal(Quote quote, QuoteWithSignal quoteWithSignal = null)
        {
            if (quoteWithSignal == null)
            {
                quoteWithSignal = _mapper.Map<QuoteWithSignal>(quote);
            }
            else
            {
                quoteWithSignal = _mapper.Map<Quote, QuoteWithSignal>(quote, quoteWithSignal);
            }

            return quoteWithSignal;
        }

        public static QuoteWithSignal MapSignalToQuoteWithSignal(Signal signal, QuoteWithSignal quoteWithSignal = null)
        {
            if (quoteWithSignal == null)
            {
                quoteWithSignal = _mapper.Map<QuoteWithSignal>(signal);
            }
            else
            {
                quoteWithSignal = _mapper.Map<Signal, QuoteWithSignal>(signal, quoteWithSignal);
            }

            return quoteWithSignal;
        }

        public static List<QuoteWithSignal> MapQuoteListToQuoteWithSignalList(List<Quote> quotes, List<QuoteWithSignal> quotesWithSignal = null)
        {
            if (quotesWithSignal == null)
            {
                quotesWithSignal = _mapper.Map<List<QuoteWithSignal>>(quotes);
            }
            else
            {
                quotesWithSignal = _mapper.Map<List<Quote>, List<QuoteWithSignal>>(quotes, quotesWithSignal);
            }

            return quotesWithSignal;
        }

        public static List<QuoteWithSignal> MapSignalListToQuoteWithSignalList(List<Signal> signal, List<QuoteWithSignal> quotesWithSignal = null)
        {
            if (quotesWithSignal == null)
            {
                quotesWithSignal = _mapper.Map<List<QuoteWithSignal>>(signal);
            }
            else
            {
                quotesWithSignal = _mapper.Map<List<Signal>, List<QuoteWithSignal>>(signal, quotesWithSignal);
            }

            return quotesWithSignal;
        }

        public static Quote MapQuoteWithSignalToQuote(QuoteWithSignal quoteWithSignal)
        {
            return _mapper.Map<Quote>(quoteWithSignal);
        }

        public static Signal MapQuoteWithSignalToSignal(QuoteWithSignal quoteWithSignal)
        {
            return _mapper.Map<Signal>(quoteWithSignal);
        }

        public static List<Quote> MapQuoteWithSignalListToQuoteList(List<QuoteWithSignal> quoteWithSignal)
        {
            List<Quote> quotes = _mapper.Map<List<Quote>>(quoteWithSignal);
            return quotes;
        }

        public static List<Signal> MapQuoteWithSignalListToSignalList(List<QuoteWithSignal> quoteWithSignal)
        {
            List<Signal> signals = _mapper.Map<List<Signal>>(quoteWithSignal);
            return signals;
        } 
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Quote, QuoteWithSignal>();
            CreateMap<Signal, QuoteWithSignal>();
            CreateMap<List<Quote>, List<QuoteWithSignal>>();
            CreateMap<List<Signal>, List<QuoteWithSignal>>();

            CreateMap<QuoteWithSignal, Quote>();
            CreateMap<QuoteWithSignal, Signal>();
            CreateMap<List<QuoteWithSignal>, List<Quote>>();
            CreateMap<List<QuoteWithSignal>, List <Signal>>();
        }
    }
}
