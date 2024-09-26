using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new();
factory.Uri = new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P (Point-toPoint) Tasarımı
//Exchange type belirtilmeyen bir queue'da default direct exchange kullanılır 
//string queueName = "example-p2p-queue";

//channel.QueueDeclare(queue: queueName, false, exclusive: false, autoDelete: false);

//EventingBasicConsumer consumer = new(channel);

//channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

//consumer.Received += Consumer_Received;

//void Consumer_Received(object? sender, BasicDeliverEventArgs e)
//{
//    Console.WriteLine($"Gelen mesaj: {Encoding.UTF8.GetString(e.Body.Span)}");
//}

#endregion

#region Publish/Subscribe (Pub/Sub) Tasarımı

//string exchangeName = "pub-sub-exhange-name";

//channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

//string queueName = channel.QueueDeclare().QueueName;
//channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: string.Empty);

////Ölçeklendime fonksiyonu
////tüm consumer'ların o anda sadce 1 tane mesajı işleyeceklerini ve toplam mesaj boyutu olarak  sınırsız mesaj alabileceklerini ifade eder
////channel.BasicQos(prefetchCount:1,prefetchSize:0,global:false);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

//consumer.Received += ConsumerReceived;

//void ConsumerReceived(object? sender, BasicDeliverEventArgs e)
//{
//    Console.WriteLine($"Gelen mesaj: {Encoding.UTF8.GetString(e.Body.Span)}");
//}

#endregion

#region Work Queue(İş Kuyruğu) Tasarımı
//Work Queue tasarımında genellikle direct exchange kullanılmaktadır.

//string queueName = "work-queue-example";

//channel.QueueDeclare(queue: queueName, false, false, false);

//EventingBasicConsumer consumer = new(channel);

////her mesajın 1 consumer tarafından tüketilebilmesi için autoAck özelliğine true değeri verildi.
//channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

//channel.BasicQos(prefetchSize: 1, prefetchCount: 0, global: false);
//consumer.Received += ConsumerReceived;

//void ConsumerReceived(object? sender, BasicDeliverEventArgs e)
//{
//    Console.WriteLine($"Gelen mesaj: {Encoding.UTF8.GetString(e.Body.Span)}");
//}

#endregion

#region Request/Response Tasarımı

string requestQueueName = "request-response-queue-example";

channel.QueueDeclare(queue: requestQueueName, durable: true, exclusive: true, autoDelete: true);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: requestQueueName, autoAck: true, consumer: consumer);

consumer.Received += Consumer_Received;

void Consumer_Received(object? sender, BasicDeliverEventArgs e)
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);

    byte[] responseMessage = Encoding.UTF8.GetBytes($"İşlem tamamlandı. : {message}");
    IBasicProperties properties = channel.CreateBasicProperties();
    channel.BasicPublish(exchange: string.Empty, routingKey: e.BasicProperties.ReplyTo, basicProperties: properties, body: responseMessage);

}

#endregion


Console.Read();