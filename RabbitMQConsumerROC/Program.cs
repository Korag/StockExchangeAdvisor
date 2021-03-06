﻿using TechnicalIndicators;
using RabbitMQ;

namespace RabbitMQConsumerROC
{
    public class RabbitConsumerCalculateROC
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
            TechnicalIndicator _indicator = new ROC();
            InititalizeParameters(_indicator.GetType().ToString());

            RabbitCalculateIndicator rabbitROC = new RabbitCalculateIndicator(_exchange, _queueReceiveFrom, _queueSendTo, _indicator);
            rabbitROC.ConsumeData();
        }
    }
}
