// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// This application uses the Azure IoT Hub device SDK for .NET
// For samples see: https://github.com/Azure/azure-iot-sdk-csharp/tree/master/iothub/device/samples

using System;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace simulatedDevice
{
    class SimulatedDevice
    {
        private static DeviceClient s_deviceClient;

        // The device connection string to authenticate the device with your IoT hub.
        private const string s_connectionString = "HostName=IotHubDoJefersonLeal.azure-devices.net;DeviceId=DeviceCaminhoesBR01;SharedAccessKey=5MJP39G4WhkEEDWm4fm8IEkRSkckmzqEyJfTHsRxXYw=";

        // Async method to send simulated telemetry
        private static async void SendDeviceToCloudMessagesAsync()
        {
            //// Initial telemetry values
            //double minTemperature = 20;
            //double minHumidity = 60;

            int maxLatitude = 180;
            int minLatitude = -180;
            int maxLongitude = 90;
            int minLongitude = -90;
            

            Random rand = new Random();

            while (true)
            {
                //double currentTemperature = minTemperature + rand.NextDouble() * 15;
                //double currentHumidity = minHumidity + rand.NextDouble() * 20;
                double currentLatitude = rand.Next(minLatitude, maxLatitude);
                double currentLongitude = rand.Next(minLongitude, maxLongitude);
                
                //// Create JSON message
                //var telemetryDataPoint = new
                //{
                //    temperature = currentTemperature,
                //    humidity = currentHumidity
                //};
                
                var telemetria = new 
                {
                    latitude = currentLatitude,
                    longitude = currentLongitude,
                    deviceId = "Dispositivo01"
                };
                
                
                //var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var messageString = JsonConvert.SerializeObject(telemetria);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                // Add a custom application property to the message.
                // An IoT hub can filter on these properties without access to the message body.
                // message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                // Send the tlemetry message
                await s_deviceClient.SendEventAsync(message).ConfigureAwait(false);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);


                await Task.Delay(10000).ConfigureAwait(false);
            }
        }

        private static void Main()
        {
            Console.WriteLine("IoT Hub Quickstarts - Simulated device. Ctrl-C to exit.\n");

            // Connect to the IoT hub using the MQTT protocol
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString, TransportType.Mqtt);
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
    }
}
