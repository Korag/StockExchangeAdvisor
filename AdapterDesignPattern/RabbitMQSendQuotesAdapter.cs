﻿using Models;
using RabbitMQ;
using System;
using System.Collections.Generic;

namespace AdapterDesignPattern
{
    public class RabbitMQSendQuotesAdapter : IRabbitMQSendQuotesAdapter
    {
        private IRabbitMQSendQuotesToCalculation _producer { get; set; }

        public void SendQuotesToCalculationInConsumer(List<Quote> quotes, Parameters parameters,
                                                      string exchange, string queueSendTo)
        {
            IndicatorCalculationElements elements = new IndicatorCalculationElements
            {
                Quotes = quotes,
                Parameters = parameters
            };

            InitializeProducer(exchange, queueSendTo);
            _producer.Process(elements);
        }

        private void InitializeProducer(string exchange, string queueSendTo)
        {
            _producer = new RabbitMQSendQuotesToCalculation(exchange, queueSendTo);
        }
    }
}
