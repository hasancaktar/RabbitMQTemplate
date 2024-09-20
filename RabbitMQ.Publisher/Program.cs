using System.Text;
using RabbitMQ.Client;

//Bağlantı oluşturma
ConnectionFactory factory = new();

//Bağlantıyı Aktifleştirme ve akanal açma
factory.Uri = new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//queue oluşturma
channel.QueueDeclare(queue: "test-queue", exclusive: false);


//queue'ye mesaj gönderme
//RabbitMQ kuyrupa atacağı mesajları byte türünden kabul ediyor. O yüzden mesajları byte'a dönüştürmemiz gerekiyor.
byte[] message = Encoding.UTF8.GetBytes("TEST MESAJ");
channel.BasicPublish(exchange: "", "test-queue", body: message);


Console.Read();
