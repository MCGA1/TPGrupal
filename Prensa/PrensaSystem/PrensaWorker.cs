using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prensa.PrensaSystem;
using System.ComponentModel;
using Prensa.Control;
using System.Text;
using CommonDomain;
using Serilog;

namespace Prensa.Controllers
{
    public static class PrensaWorker
    {
        static readonly ConnectionFactory _connectionFactory;
        static readonly string _consumerQueue;
        static bool _state = true;
        public static bool State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;

                if (worker == null)
                {
                    worker = new BackgroundWorker();
                    worker.DoWork += MainLoop;
                }


                if (value && worker.IsBusy != true)
                {
                    worker.RunWorkerAsync();
                }

                if (worker.IsBusy != true)
                {
                    worker.CancelAsync();
                }
            }
        }
        static BackgroundWorker worker;

        static PrensaWorker()
        {
            // Configurar el rabbit
            _connectionFactory = new ConnectionFactory() { HostName = "localhost", DispatchConsumersAsync = true };
            _consumerQueue = "Cinta";
            State = true;
        }

        public static async Task<IModel> TryConnectToRabbit()
        {
            while (!worker.CancellationPending)
            {
                try
                {
                    IModel channel;
                    Log.Information($"Worker: Intentando conectar a RabbitMQ en la cola {_consumerQueue}...");
                    channel = _connectionFactory.CreateConnection().CreateModel();
                    channel.QueueDeclare(queue: _consumerQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    Log.Information("Worker: Conexion exitosa, continuando...");
                    return channel;
                }
                catch (Exception)
                {
                    Log.Error($"\nWorker: Error al conectar con el servicio de rabbitMQ en la cola '{_consumerQueue}', reintentando...");
                    await Task.Delay(3000);
                }

            }

            return null;
        }


        public static async void MainLoop(object sender, DoWorkEventArgs args)
        {
            var channel = await TryConnectToRabbit();


                 
            while (!worker.CancellationPending)
            {
                Log.Information("\nBuscando bultos disponibles...");
                if (channel.MessageCount(_consumerQueue) == 0)
                {
                    Log.Information("No se encontraron bultos, volviendo a intentar...");
                    await Task.Delay(2000);
                    continue;
                }

                Log.Information("Worker: Bulto encontrado.");

                //var item = channel.BasicConsume(_consumerQueue, autoAck: true, consumer);
                BasicGetResult result = channel.BasicGet(_consumerQueue, true);
                var bulto = await ParseBulto(result);
                Log.Information("Worker: Bulto recibido.");


                var _bultoProcesado = await MaquinaPrensado.Prensar(bulto);
                Log.Information("Worker: Moviendo y guardando bulto procesado...");

                try
                {
                    // TODO: Guardar bulto en base de datos
                    await GuardarBultoProcesado(_bultoProcesado);
                    Log.Information("Bulto procesado guardado.");
                }
                catch (Exception ex)
                {
                    Log.Error($"No se pudo procesar el bulto. Mensaje de error: {ex.Message}");
                    throw;
                }


                await Task.Delay(1000);
            }

        }


        private static async Task<Bulto> ParseBulto(BasicGetResult result)
        {
            if (result == null)
            {
                return null;
            }
            else
            {
                IBasicProperties props = result.BasicProperties;
                ReadOnlyMemory<byte> body = result.Body;

                var json = Encoding.UTF8.GetString(body.ToArray());

                var bulto = JsonConvert.DeserializeObject<Bulto>(json);

                return bulto;
            }

        }

        public static async Task GuardarBultoProcesado(BultoProcesado bulto)
        {
            BultoControl.LlevarBultoALaPila(bulto);
        }





    }
}
