using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StateAndDecoratorDesignPattern;
using System;
using System.Collections.Generic;
using WebServicesModels;

namespace Utility
{
    public static class JsonSerializer
    {
        public static string CollectionOfQuotesToJsonString(List<Quote> quotes)
        {
            return JsonConvert.SerializeObject(quotes);
        }

        public static List<Quote> JsonStringToCollectionOfQuotes(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<Quote>>(jsonString);
        }

        public static string CollectionOfSignalsToJsonString(List<Signal> signals)
        {
            return JsonConvert.SerializeObject(signals);
        }

        public static List<Signal> JsonStringToCollectionOfSignals(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<Signal>>(jsonString);
        }

        public static string CollectionOfIndicatorCalculationElementsWIndicatorTypeToJsonString(IndicatorCalculationElementsWIndicatorType indicatorElements)
        {
            return JsonConvert.SerializeObject(indicatorElements);
        }

        public static string CollectionOfQuotesWithParametersToJsonString(IndicatorCalculationElements indicatorElements)
        {
            return JsonConvert.SerializeObject(indicatorElements);
        }

        public static IndicatorCalculationElements JsonStringToCollectionOfQuotesWithParameters(string jsonString)
        {
            return JsonConvert.DeserializeObject<IndicatorCalculationElements>(jsonString);
        }

        public static string DateTimeToJsonString(DateTime dateTime)
        {
            return JsonConvert.SerializeObject(dateTime);
        }

        public static List<T> JsonStringToCollectionOfTypes<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<T>>(jsonString, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public static int JsonStringToInt(string jsonString)
        {
            return JsonConvert.DeserializeObject<int>(jsonString);
        }

        public static DateTime JsonStringToDateTime(string jsonString)
        {
            return JsonConvert.DeserializeObject<DateTime>(jsonString);
        }

        public static string SignalModelContextToJsonString(SignalModelContext signalContext)
        {
            return JsonConvert.SerializeObject(signalContext);
        }

        public static SignalModelContext JsonStringToSignalModelContext(string jsonString)
        {
            return JsonConvert.DeserializeObject<SignalModelContext>(jsonString);
        }

        public static string SignalModelContextListToJsonString(List<SignalModelContext> signalContext)
        {
            return JsonConvert.SerializeObject(signalContext, Formatting.Indented,
                                              new JsonSerializerSettings
                                              {
                                                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                              });
        }

        public static List<SignalModelContext> JsonStringToSignalModelContextList(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<SignalModelContext>>(jsonString);
        }
    }
}
