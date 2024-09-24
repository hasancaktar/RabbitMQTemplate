using System.Text;
using RabbitMQ.Client;

//Bağlantı oluşturma
ConnectionFactory factory = new();

//Bağlantıyı Aktifleştirme ve akanal açma
factory.Uri = new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//queue oluşturma
//durable parametresi Message Durability için. fiziksel olarak kuyruğun kalıcı olabilmesi için
channel.QueueDeclare(queue: "test-queue", exclusive: false, durable: true);

//Mesajların kalıcı olabilmesi için konfigürasyon
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;


//queue'ye mesaj gönderme
//RabbitMQ kuyrupa atacağı mesajları byte türünden kabul ediyor. O yüzden mesajları byte'a dönüştürmemiz gerekiyor.
for (int i = 0; i < 100; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("TEST MESAJ " + i);

    channel.BasicPublish(exchange: "", "test-queue", body: message, basicProperties: properties);
    await Task.Delay(200);
}



Console.Read();
