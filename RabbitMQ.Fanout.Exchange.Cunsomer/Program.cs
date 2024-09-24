using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");

using IConnection connection = factory.CreateConnection();

//RabbitMQ ile mesaj alışverişi yapmak için kullanılan bir kanal (channel) oluşturur. Bu kanal üzerinden mesajlar gönderilip alınır.
using IModel channel = connection.CreateModel();


channel.ExchangeDeclare("direct-exchange-example", ExchangeType.Fanout);

Console.WriteLine("Kuyruk adı gir: ");
string queueName = Console.ReadLine();


channel.QueueBind(queue: queueName, exchange: "fanout-exchange-example", routingKey: string.Empty);

EventingBasicConsumer consumer = new(channel);

//belirtilen kuyruktaki mesajları almak için kullanılır.
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

consumer.Received += ConsumerReceived;

void ConsumerReceived(object? sender, BasicDeliverEventArgs e)
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine($"Gelen Mesaj: {message}");
}