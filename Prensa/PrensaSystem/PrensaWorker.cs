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

namespace Prensa.Controllers
{
    public static class PrensaWorker
    {
        static readonly ConnectionFactory _connectionFactory;
        static readonly string _consumerQueue;
        static bool _state = true;
        static bool State 
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


                if (value)
                {
                    worker.RunWorkerAsync();
                }

                else
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

        public static async void MainLoop(object sender, DoWorkEventArgs args)
        {
            
            var channel = _connectionFactory.CreateConnection().CreateModel();

            var consumer = new AsyncEventingBasicConsumer(channel);

            while(!worker.CancellationPending)
            {
                Console.WriteLine("\nBuscando bultos disponibles...");
                if (channel.MessageCount(_consumerQueue) == 0)
                {
                    Console.WriteLine("No se encontraron bultos, volviendo a intentar...");
                    await Task.Delay(2000);
                    continue;
                }

                Console.WriteLine("Worker: Bulto encontrado.");
                
                var bulto = JsonConvert.DeserializeObject<Bulto>(channel.BasicConsume(_consumerQueue, autoAck:true, consumer));            
                Console.WriteLine("Worker: Bulto recibido.");


                var _bultoProcesado = await MaquinaPrensado.Prensar(bulto);
                Console.WriteLine("Worker: Moviendo y guardando bulto procesado...");

                try
                {
                    // TODO: Guardar bulto en base de datos
                    await GuardarBultoProcesado(_bultoProcesado);
                    Console.WriteLine("Bulto procesado guardado.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"No se pudo procesar el bulto. Mensaje de error: {ex.Message}");
                    throw;
                }
                

                await Task.Delay(1000);
            }

        }



        public static async Task GuardarBultoProcesado(BultoProcesado bulto)
        {
            CommonServices.Context.BultoStorageService.SaveBultos(bulto);
        }





    }
}
