using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new();
factory.Uri=new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");


using IConnection connection = factory.CreateConnection();

//RabbitMQ ile mesaj alışverişi yapmak için kullanılan bir kanal (channel) oluşturur. Bu kanal üzerinden mesajlar gönderilip alınır.
using IModel channel = connection.CreateModel();


channel.ExchangeDeclare("direct-exchange-example", ExchangeType.Direct);

string queueName = channel.QueueDeclare().QueueName;


//QueueBind() metodu, bir kuyruk ile bir exchange'i bağlar.
//queue: queueName: Bu, daha önce otomatik olarak oluşturulan kuyruk.
//exchange: "direct-exchange-example": Bu exchange, yukarıda tanımlanan exchange'dir.
//routingKey: "example-routing": Mesajları bu anahtarla yönlendirilecek şekilde kuyruğa bağlar.
channel.QueueBind(queue:queueName,exchange: "direct-exchange-example", routingKey: "example-routing");

//RabbitMQ'dan mesajları almak için kullanılan bir consumer sınıfıdır. Bu sınıf, mesaj alındığında bir olay (event) oluşturur.
//channel, bu consumer'ın hangi kanalı kullanacağını belirler. Burada, yukarıda oluşturulan kanal kullanılıyor.
EventingBasicConsumer consumer = new(channel);

//BasicConsume(), belirtilen kuyruktaki mesajları almak için kullanılır.
//autoAck: true: Mesajın otomatik olarak RabbitMQ'ya alındığı bilgisini gönderir. Bu, mesajın başarıyla alındığını RabbitMQ'ya bildirir.
//queue: queueName: Tüketilecek olan kuyruğun adı.
//consumer: consumer: Bu consumer, mesajları işleyip kuyruktan alır.
channel.BasicConsume(queue: queueName, autoAck: true,consumer: consumer);

consumer.Received += Consumer_Received;

void Consumer_Received(object? sender, BasicDeliverEventArgs e)
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine($"Gelen mesaj: {message}");
}

Console.Read();