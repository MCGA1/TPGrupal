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
using Prensa.SensoresSystem;
using NetMQ.Sockets;
using NetMQ;
using System.Threading;
using CommonServices.Entities.Enum;

namespace Prensa.Controllers
{
    public static class PrensaWorker
    {
        static readonly ConnectionFactory _connectionFactory;
        static readonly string _consumerQueue;
        static bool _state = true;
        static CancellationTokenSource cts = new CancellationTokenSource();
        
        static bool WorkerIsActive { get; set; }

        public static bool State
        {
            get
            {
                return (worker != null && !worker.CancellationPending && worker.IsBusy) ? true
                    : (worker == null) ? false
                    : (worker.CancellationPending) ? false
                    : (!worker.IsBusy) ? false : true;
            }
            set
            {
                _state = value;

                

                if (worker == null)
                {
                    worker = new BackgroundWorker();
                    worker.DoWork += MainLoop;
                    worker.WorkerSupportsCancellation = true;
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
            WorkerIsActive = true;

            var channel = await TryConnectToRabbit();

            bool activesensor = true;
                 
            while (!worker.CancellationPending)
            {
                while (true)
                {
                    try
                    {
                        while (true)
                        {
                            if (SensorPasivo.IsPaused())
                            {
                                Log.Information("Proceso pausado.");
                                await Task.Delay(2000);
                                continue;
                            }
                            break;
                        }

                        Log.Information("Esperando que el sensor activo esté listo...");
                        activesensor = await SensorActivoCommunicator.GetStatus();
                        if (!SensorPasivo.IsPaused() && SensorPasivo.GetStatus() && activesensor)
                        {
                            break;
                        }
                    
                        await Task.Delay(2000);
                    }
                    catch (Exception ex)
                    {
                        Log.Information(ex.Message);
                    }
                }

                Log.Information("\nBuscando bultos disponibles...");
                if (channel.MessageCount(_consumerQueue) == 0)
                {
                    Log.Information("No se encontraron bultos, volviendo a intentar...");
                    await Task.Delay(2000);
                    continue;
                }

                Log.Information("Worker: Bulto encontrado.");

                BasicGetResult result = channel.BasicGet(_consumerQueue, false);
                
                var bulto = await ParseBulto(result);
                Log.Information("Worker: Bulto recibido.");


                await MaquinaPrensado.StoreBulto(bulto);
                await MaquinaPrensado.Prensar();
                Log.Information("Worker: Moviendo y guardando bulto procesado...");

                try
                {
                    await GuardarBultoProcesado();
                    channel.BasicAck(result.DeliveryTag, false);
                    Log.Information("Bulto procesado guardado.");
                }
                catch (Exception ex)
                {
                    Log.Error($"No se pudo procesar el bulto. Mensaje de error: {ex.Message}");
                    throw;
                }


                await Task.Delay(2000);
            }

            WorkerIsActive = false;

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

        public static async Task GuardarBultoProcesado()
        {
            var bulto = await MaquinaPrensado.RemoveBulto();
            BultoControl.LlevarBultoALaPila(bulto);
        }

        public static void SetStatus(ServiceStatus status)
        {
            if (status == ServiceStatus.Running && worker != null && !WorkerIsActive)
                State = true;

            else if (status == ServiceStatus.Stopped && worker != null)
                State = false;
        }

    }
}
