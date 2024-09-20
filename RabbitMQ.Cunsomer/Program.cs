using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");

//Bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Queue oluşturma
channel.QueueDeclare(queue: "test-queue", exclusive: false); //Cunsomer'da d da kuyruk publisher'daki ile birebir aynı yapılandırmada tanımlanmalıdır.

//Queue'dan mesaj okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "test-queue", false, consumer);

consumer.Received += Consumer_Received;

void Consumer_Received(object? sender, BasicDeliverEventArgs e)
{
    //Kuyruğa gelen mesajın işlendiği yer.
    //e.body : kuyruktaki mesajın verisini bütünsel olarak getirir.
    //e.body.span veya e.body.Array() : Kuyruktaki mesajın byte verisini getirir.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
}
Console.Read();