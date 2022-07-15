using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KIOSKController.services
{
    internal class MqttClient
    {
        private readonly MqttFactory Factory;
        private readonly IMqttClient Client;

        readonly string cliendId = ConfigurationManager.AppSettings["mqttClientId"];
        readonly string ip = ConfigurationManager.AppSettings["mqttEndpoint"];
        readonly int port = Int32.Parse(ConfigurationManager.AppSettings["mqttPort"]);
        readonly string username = ConfigurationManager.AppSettings["mqttUsername"];
        readonly string password = ConfigurationManager.AppSettings["mqttPassword"];

        public MqttClient()
        {
            var factory = new MqttFactory();
            Factory = factory;
            Client = factory.CreateMqttClient();
        }

        public void Connect()
        {
            MqttClientOptions options;
            var builder = new MqttClientOptionsBuilder()
                .WithClientId(cliendId)
                .WithTcpServer(ip, port)
                .WithCleanSession();

            if (password != "")
            {
                builder.WithCredentials(username, password);
            }
            else if (username != "")
            {
                builder.WithCredentials(username);
            }
    
            options = builder.Build();

            try
            {
                Client.ConnectAsync(options).Wait();
            }
            catch (Exception)
            {
                throw new Exception("Can't connect to MQTT broker.");
            }

            Console.WriteLine($"Mqtt server connected at {ip}:{port}");
        }

        public async Task Publish(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build();

            if (!Client.IsConnected)
            {
                return;
            }

            await Client.PublishAsync(message);
        }

        public async Task Subscribe(string topic, Action<string> callback)
        {
            Client.ApplicationMessageReceivedAsync += e =>
            {
                Console.WriteLine("Received application message.");

                var payload = Encoding.Default.GetString(e.ApplicationMessage.Payload);
                callback(payload);

                return Task.CompletedTask;
            };

            var mqttSubscribeOptions = Factory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic(topic); })
                .Build();

            await Client.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

            Console.WriteLine($"MQTT client subscribed to {topic}.");
        }

        private void SimulatePublish()
        {
            Console.WriteLine("Press key to publish message.");
            Console.ReadLine();

            var counter = 0;

            while (counter < 10)
            {
                counter++;

                var testMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("test")
                    .WithPayload($"Payload: {counter}")
                    .Build();

                if (Client.IsConnected)
                {
                    Console.WriteLine($"publishing at {DateTime.UtcNow}");
                    Client.PublishAsync(testMessage);
                }
                Thread.Sleep(2000);
            }

            Console.WriteLine("Simulation ended! press any key to exit.");
            Client.DisconnectAsync().Wait();
            Console.ReadLine();
        }
    }
}
