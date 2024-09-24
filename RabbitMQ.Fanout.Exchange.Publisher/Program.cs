using System.Text;
using RabbitMQ.Client;

ConnectionFactory factory = new();
factory.Uri = new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


//Fanout Exchange: Mesajları tüm bağlı kuyruklara yönlendirir. Routing key kullanmaz; yani gelen mesajları indis gözetmeksizin tüm bağlı kuyruklara gönderir. tv yayını gibi
channel.ExchangeDeclare(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    channel.BasicPublish(exchange:"fanout-exchange-example", routingKey: string.Empty, body:message);

}

Console.Read();