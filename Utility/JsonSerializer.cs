using Models;
using Newtonsoft.Json;
using System.Collections.Generic;

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

        //public static string MessageToJsonString(Message message)
        //{
        //    return JsonConvert.SerializeObject(message);
        //}

        //public static Message JsonStringToMessage(string jsonString)
        //{
        //    return JsonConvert.DeserializeObject<Message>(jsonString);
        //}
    }
}
