﻿using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;

namespace Dotpay.Actor
{
    public interface IMessageQueueProducter : IGrainWithIntegerKey
    {
        Task RegisterAndBindQueue(string exchange, string exchangeType, string queue, string routeKey, bool durable);
        Task PublishMessage(MqMessage message, string exchangeName, string routeKey, bool durable);
    }

    [Immutable]
    [Serializable]
    public class MqMessage
    {
    }
}
