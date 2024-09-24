using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


//RabbitMQ'da bir exchange (değişim) tanımlamak için kullanılır. Exchange, RabbitMQ'da mesajların kuyruklara (queue) yönlendirilmesinden sorumlu bir yapıdır. RabbitMQ'ya gelen mesajlar doğrudan bir kuyruğa gitmez; önce bir exchange'e gelir, sonra bu exchange mesajı uygun kuyruğa yollar.
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while (true)
{
    Console.WriteLine("Mesaj: ");
   string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchange: "direct-exchange-example", routingKey: "example-routing", body: byteMessage);
}



Console.Read();