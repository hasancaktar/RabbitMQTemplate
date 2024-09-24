﻿using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://rpvitboc:BJQYh4M3859bK5fRTtE1oZl_qHrr36ZN@moose.rmq.cloudamqp.com/rpvitboc");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare("topic-exchange-example", ExchangeType.Topic);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    Console.Write("Mesajın gönderileceği topic formatını belirtiniz: ");
    string topic = Console.ReadLine();
    channel.BasicPublish(exchange: "topic-exchange-example", routingKey: topic, body: message);
}

Console.Read();