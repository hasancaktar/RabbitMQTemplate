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
channel.QueueDeclare(queue: "test-queue", exclusive: false,durable:true); //Cunsomer'da d da kuyruk publisher'daki ile birebir aynı yapılandırmada tanımlanmalıdır.

//Queue'dan mesaj okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "test-queue",autoAck: false, consumer);
//prefetchSize, bir cnsomer tarafından alınabilecek en büyük mesaj boyutubu byte cinsinden belirler. 0 sınırsız demektir. prefetchCount, bir cunsomer tarafından aynı anda işleme alınabilecek mesaj sayısını belirler, global, tüm cunsomerlar için mi yoksa sadece çağrı yapılan cunsomer için mi geçerli olacağını belirler.
channel.BasicQos(prefetchSize:0,prefetchCount:1,global:false);

consumer.Received += Consumer_Received;

void Consumer_Received(object? sender, BasicDeliverEventArgs e)
{
    //Kuyruğa gelen mesajın işlendiği yer.
    //e.body : kuyruktaki mesajın verisini bütünsel olarak getirir.
    //e.body.span veya e.body.Array() : Kuyruktaki mesajın byte verisini getirir.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    channel.BasicAck(deliveryTag:e.DeliveryTag,multiple:false);
}
Console.Read();