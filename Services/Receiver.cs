using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Server.Models;

namespace Server.Services
{
    public class Receiver : IReceiver
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _consumer;
        private readonly IHubContext<MessageHub> _hubContext;

        private const int MIN_PRIORITY = 7;

        public Receiver(IHubContext<MessageHub> hubContext)
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _consumer = new EventingBasicConsumer(_channel);
            _hubContext = hubContext;
        }

        public void Receive()
        {
            _channel.QueueDeclare(queue: "message",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var messageJson = JsonSerializer.Deserialize<Response>(message);
                if (messageJson?.Priority >= MIN_PRIORITY)
                {
                    _hubContext.Clients.All.SendAsync(
                        "SendMessage", 
                        $"[{messageJson.TimeStamp}] : {messageJson.Priority} - {messageJson.Message}"
                    );
                }
            };
            
            _channel.BasicConsume(queue: "message",
                                autoAck: true,
                                consumer: _consumer);
        }
    }
}