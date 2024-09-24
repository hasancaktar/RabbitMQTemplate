using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new();
factory.Uri = new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");

IConnection connection = factory.CreateConnection();
IModel channel = connection.CreateModel();

channel.ExchangeDeclare("header-exchange-example", ExchangeType.Headers);

Console.Write($"Header value'sunu gir: ");
string value = Console.ReadLine();

//channel.QueueDeclare() koduna kadar bir random queue oluşturuyor. sonra QueueName ile o kuyruğun ismi alınıyor.
string queueName = channel.QueueDeclare().QueueName;


channel.QueueBind(
    queueName, "header-exchange-example",
    string.Empty,
    arguments: new Dictionary<string, object> { ["no"] = value });

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(queueName, autoAck: true, consumer: consumer);
consumer.Received += Consumer_Received;

void Consumer_Received(object? sender, BasicDeliverEventArgs e)
{
    Console.WriteLine($"Gelen mesaj: {Encoding.UTF8.GetString(e.Body.Span)}");
}

Console.Read();