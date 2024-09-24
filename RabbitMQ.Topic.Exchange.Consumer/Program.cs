using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "topic-exchange-example", ExchangeType.Topic);
Console.WriteLine("Dinlenecek topic formatını belirt: ");
string topic = Console.ReadLine();
string queueNmae = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueNmae, exchange: "topic-exchange-example", routingKey: topic);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueNmae, autoAck: true, consumer: consumer);

consumer.Received += ConsumerReceived;

void ConsumerReceived(object? sender, BasicDeliverEventArgs e)
{
    Console.WriteLine($"Gelen mesaj: {Encoding.UTF8.GetString(e.Body.Span)}");
}


Console.Read();