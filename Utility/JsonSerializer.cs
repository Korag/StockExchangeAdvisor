using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Utility
{
    public static class JsonSerializer
    {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static string ConvertObjectToJsonString<T>(T model)
        {
            return JsonConvert.SerializeObject(model, _settings);
        }

        public static string ConvertCollectionOfObjectsToJsonString<T>(List<T> model)
        {
            return JsonConvert.SerializeObject(model, _settings);
        }

        public static T JsonStringToObjectType<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString, _settings);
        }

        public static List<T> JsonStringToCollectionOfObjectsTypes<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<T>>(jsonString, _settings);
        }

        //public static string SignalModelContextListToJsonString(List<SignalModelContext> signalContext)
        //{
        //    return JsonConvert.SerializeObject(signalContext, Formatting.Indented,
        //                                      new JsonSerializerSettings
        //                                      {
        //                                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //                                      });
        //}
    }
}
