using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P (Point-toPoint) Tasarımı

//string queueName = "example-p2p-queue";

//channel.QueueDeclare(queue:queueName,false,exclusive:false,autoDelete:false);

//byte[] message = "Merhaba"u8.ToArray();

//channel.BasicPublish(exchange: string.Empty, routingKey: queueName, body: message);

#endregion

#region Publish/Subscribe (Pub/Sub) Tasarımı

//string exchangeName = "pub-sub-exhange-name";
//channel.ExchangeDeclare(exchange: exchangeName, ExchangeType.Fanout);

//for (int i = 0; i < 100; i++)
//{
//    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
//    channel.BasicPublish(exchange: exchangeName, routingKey: string.Empty, body: message);


//}
#endregion

#region Work Queue(İş Kuyruğu) Tasarımı

//string queueName = "work-queue-example";


//channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

////Yayınlanan mesaj kalıcı olarak gönderilmektedir
//IBasicProperties properties = channel.CreateBasicProperties();
//properties.Persistent = true;

//for (int i = 0; i < 100; i++)
//{
//    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
//    channel.BasicPublish(exchange: string.Empty, routingKey: queueName, body: message);
//}

#endregion

#region Request/Response Tasarımı

string requestQueueName = "request-response-queue-example";
channel.QueueDeclare(queue:requestQueueName,durable:false,exclusive:false,autoDelete:false);

string replyQueueName = channel.QueueDeclare().QueueName;

string correlationId = Guid.NewGuid().ToString();

#region Request mesajını oluşturma ve gönderme

IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = replyQueueName;

for (int i = 0; i < 10; i++)
{
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    channel.BasicPublish(exchange: string.Empty,routingKey: requestQueueName,body:message, basicProperties: properties);
}



#endregion

#region Response Kuyruğu dinleme

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue:replyQueueName,autoAck:true,consumer:consumer);

consumer.Received += Consumer_Received;

void Consumer_Received(object? sender, BasicDeliverEventArgs e)
{
   if(e.BasicProperties.CorrelationId == correlationId)
    {
        string message = Encoding.UTF8.GetString(e.Body.Span);
        Console.WriteLine($"Response - {message}");
    }
}

#endregion

#endregion

Console.Read();