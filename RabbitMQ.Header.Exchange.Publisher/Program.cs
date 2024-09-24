using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");

using IConnection connection = factory.CreateConnection();
using IModel channel=connection.CreateModel();

channel.ExchangeDeclare(exchange:"header-exchange-example", type: ExchangeType.Headers);

for (int i = 0; i < 100; i++)
{
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    Console.Write($"Header value'sunu gir: ");
    string value = Console.ReadLine();

    //mesajın özelliklerini tanımlamak için
    //"no" anahtarına sahip ve kullanıcı tarafından girilen değeri (value) içeriyor.
    IBasicProperties basicProperties = channel.CreateBasicProperties();
    basicProperties.Headers = new Dictionary<string, object>
    {
        ["no"] = value
    };

    channel.BasicPublish(exchange: "header-exchange-example", routingKey: string.Empty,body: message);

}

Console.Read();