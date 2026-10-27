using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace Calculator.Services
{
    public class KafkaProducerService<K, V>
    {
        private readonly IProducer<K, V> _kafkaHandle;

        public KafkaProducerService(KafkaProducerHandler handle)
        {
            _kafkaHandle = new DependentProducerBuilder<K, V>(handle.Handle).Build();
        }

        public Task ProduceAsync(string topic, Message<K, V> message)
            => _kafkaHandle.ProduceAsync(topic, message);

        public void Produce(string topic, Message<K, V> message, Action<DeliveryReport<K, V>>? deliveryHandler = null)
            => _kafkaHandle.Produce(topic, message, deliveryHandler);

        public void Flush(TimeSpan timeout)
            => _kafkaHandle.Flush(timeout);
    }
}