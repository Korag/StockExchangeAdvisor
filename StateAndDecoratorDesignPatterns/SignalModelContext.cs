using Newtonsoft.Json;
using StateDesignPattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace StateAndDecoratorDesignPattern
{
    //State context class
    [Serializable]
    public class SignalModelContext : ICloneable
    {
        public string Date;
        public double Open;
        public double High;
        public double Low;
        public double Close;
        public int Volume;

        //public List<double> PartialSignals;
        public List<double> PartialSignals { get; set; }
        public ASignalState CurrentState = null;

        //Wykorzystanie decoratora do obliczenia
        public double FinalPrice;
        public double AdditionalFee;

        //Przeniesione do określonych stanów
        //public string Factor;
        //public int FinalSignal; -> SignalValue

        public SignalModelContext()
        {
            CurrentState = new UnknownSignalState(0, this);

            JsonSerialization = true;
            PartialSignals = new List<double>();
        }

        public bool JsonSerialization { get; set; }

        public object Clone()
        {
            if (!JsonSerialization)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    if (this.GetType().IsSerializable)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();

                        formatter.Serialize(stream, this);
                        stream.Position = 0;

                        return formatter.Deserialize(stream);
                    }

                    return null;
                }
            }
            else
            {
                string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });

                return JsonConvert.DeserializeObject<SignalModelContext>(jsonString);
            }
        }

        public void SetSignalValue(int signalValue)
        {
            CurrentState.SetSignalValue(signalValue);
        }
    }
}
