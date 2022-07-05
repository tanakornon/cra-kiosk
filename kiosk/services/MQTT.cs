using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kiosk.services
{
    class Mqtt
    {
        private readonly MqttFactory Factory;
        private readonly IMqttClient Client;

        public Mqtt()
        {
            var factory = new MqttFactory();
            Factory = factory;
            Client = factory.CreateMqttClient();
        }

        public void Connect(string cliendId, string ip, int port)
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId(cliendId)
                .WithTcpServer(ip, port)
                .WithCleanSession()
                .Build();

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

            // Console.WriteLine("Press enter to exit.");
            // Console.ReadLine();
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
