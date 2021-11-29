using CintaApi.Extensions;
using CintaApi.Interfaces;
using CintaApi.Models;
using CommonServices.Context;
using CommonServices.Entities.Enum;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CintaApi.Services
{
    public static class ServiceBusMessageService
    {
        private static IConfiguration configuration;
        private static BackgroundWorker _backgroundWorker;
        private static Extensions.Queue<Bulto> queue = new Extensions.Queue<Bulto>();
        private  static int _number = 10;
        private static  int _defaultPort = 5001;
        public static bool _paused=false;

        static ServiceBusMessageService()
        {
            
        }

        public static void Init()
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += PublicarBulto;
            _backgroundWorker.RunWorkerAsync();
        }


        public static bool  SetStatus(ServiceStatus serviceStatus)
        {
            if (serviceStatus==ServiceStatus.Running)
            {
                return true;
            }
            return false;
        }


        public static Task SetVelocity(int number)
        {
            _number = number;
            return Task.CompletedTask;
        }

        public static void PonerBulto(object obj, DoWorkEventArgs args)
        {
            queue.Dequeue();
        }

        public static async Task PonerBulto(Bulto entity)
        {

            await Task.Run(() => queue.Enqueue(entity));
        }

        public static Task IntitializeProcess()
        {
            Init();
            return Task.CompletedTask;
        }

        public static Task StopProcess()
        {
            if (_backgroundWorker.CancellationPending==false)
            {
                _backgroundWorker.CancelAsync();
            }

            return Task.CompletedTask;
        }

        public static async Task PonerBulto(IEnumerable<Bulto> entity)
        {

            foreach (var item in entity)
            {
                queue.Enqueue(item);


                var bulto = new BultoIngresado()
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Peso = item.Peso,
                    Enviado = false,
                    Fecha=DateTime.Now
                };

                BultoIngresadoService.SaveBultoIngresado(bulto);



            }
            await Task.Delay(1);
        }

        public static async void PublicarBulto(object obj, DoWorkEventArgs args)
        {
            var connection = ConnectionFactoryExtensions.ConnectionFactory();

            var channel = connection.CreateConnection().CreateModel();

            Log.Information("Worker iniciado");
            while (_backgroundWorker.CancellationPending == false)
            {
                while (true)
                {
                    if (_paused)
                    {
                        Log.Information("Cinta pausada.");
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }

                Log.Information("buscando bultos");
                await Task.Delay(1000);
                {
                    channel.QueueDeclare(queue: Contants.GetQueueName(), durable: true, exclusive: false, autoDelete: false, arguments: null);

                    //-> Mensaje

                    var items = queue.Dequeue();

                    if (items == null)
                    {
                        Log.Information("no se encontraron bultos");

                        continue;
                    }
                    Log.Information("el bulto ha sido ingresado", JsonConvert.SerializeObject(items));


                    BultoIngresadoService.UpdateBultoIngresado(items.Id,DateTime.Now);



                    var json  = JsonConvert.SerializeObject(items);


                    var body = Encoding.UTF8.GetBytes(json);

                    //-> Enviamos el mensaje
                    channel.BasicPublish(exchange: "", routingKey: Contants.GetQueueName(), basicProperties: null, body: body);


                }


            }
            channel.Dispose();
            return;
        }

        public static async Task<Bulto> GetIndididualBulto(string bultoId)
        {
            var connection = ConnectionFactoryExtensions.ConnectionFactory();


            var channel = connection.CreateConnection().CreateModel();

            {
                channel.QueueDeclare(queue: Contants.GetQueueName(), durable: false, exclusive: false, autoDelete: false, arguments: null);
                BasicGetResult result = channel.BasicGet(Contants.GetQueueName(), true);
                if (result == null)
                {
                    throw new Exception();
                }
                else
                {
                    IBasicProperties props = result.BasicProperties;
                    ReadOnlyMemory<byte> body = result.Body;

                    var json = Encoding.UTF8.GetString(body.ToArray());

                    var deseriliaze = JsonConvert.DeserializeObject<Bulto>(json);

                    if (deseriliaze.Id.ToString().Contains(bultoId)) await Task.FromResult(deseriliaze);

                }
            }
            return null;
        }

        public static async Task<List<string>> CheckQueueMensagges()
        {
            List<string> queueBultos = new List<string>();

            var connection = ConnectionFactoryExtensions.ConnectionFactory();


            var channel = connection.CreateConnection().CreateModel();
            {
                var queueDeclareResponse = channel.QueueDeclare(Contants.GetQueueName(), false, false, false, null);

                var consumer = new AsyncEventingBasicConsumer(channel);
                channel.BasicConsume(Contants.GetQueueName(), true, consumer);

                Log.Information(" [*] Processing existing messages.");

                for (int i = 0; i < queueDeclareResponse.MessageCount; i++)
                {
                    consumer.Received += async (model, ea) =>
                   {
                       var body = ea.Body.ToArray();
                       // positively acknowledge all deliveries up to
                       // this delivery tag
                       channel.BasicAck(ea.DeliveryTag, true);
                       var message = Encoding.UTF8.GetString(body);
                       Log.Information(" [x] Received {0}", message);

                       queueBultos.Add(queueDeclareResponse.MessageCount.ToString());
                   };
                }

                return queueBultos;
            }
        }

        public static Uri FormatUrl(int portNumber)
        {
            var port= _defaultPort = portNumber;


            return new Uri($"http://localhost:{port}/");
        }
           

    }
}

