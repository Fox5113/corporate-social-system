using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Models.Employee;
using WebApi.Settings;

namespace WebApi.RabbitMq
{
    public class RabbitMqListener : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private ApplicationSettings _applicationSettings;
        private readonly ILogger<RabbitMqListener> _logger;

        public RabbitMqListener(ILogger<RabbitMqListener> logger)
        {
            _applicationSettings = new ApplicationSettings();
            _logger = logger;

            var factory = new ConnectionFactory { HostName = _applicationSettings.HostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _applicationSettings.RabbitMqQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());

                    if (!string.IsNullOrEmpty(message) && RedirectToAnotherAction(message))
                        _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex.Message);
                }
            };

            _channel.BasicConsume(_applicationSettings.RabbitMqQueue, false, consumer);

            return Task.CompletedTask;
        }

        private bool RedirectToAnotherAction(string message)
        {
            if(JsonSerializer.Deserialize<List<ShortEmployeeModel>>(message) != null)
            {
                var client = new RestClient(_applicationSettings.SiteUrl);
                var request = new RestRequest("Employee/CreateOrUpdateEmployeeRange");
                request.AddParameter("jsonData", message);
                var response = client.Post(request);

                return response != null && response.IsSuccessful;
            }

            return false;
        } 

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
