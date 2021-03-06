﻿using TechnicalIndicators;
using RabbitMQ;

namespace RabbitMQConsumerEMA
{
    public class RabbitConsumerCalculateEMA
    {
        private const string _exchange = "SignalsExchange";
        private const string _queueSendTo = "ObtainedSignals";
        private static string _queueReceiveFrom;
 
        static void InititalizeParameters(string queueReceiverFrom)
        {
            _queueReceiveFrom = queueReceiverFrom;
        }

        static void Main()
        {
            TechnicalIndicator _indicator = new TechnicalIndicatorEMA();
            InititalizeParameters(_indicator.GetType().ToString());

            RabbitCalculateIndicator rabbitEMA = new RabbitCalculateIndicator(_exchange, _queueReceiveFrom, _queueSendTo, _indicator);
            rabbitEMA.ConsumeData();
        }
    }
}
