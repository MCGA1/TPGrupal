using CommonDomain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TestBultoProducer
{
    internal class Program
    {
        static BackgroundWorker worker = new BackgroundWorker();
        static bool killpending = false;

        static void Main(string[] args)
        {
            worker.DoWork += MainLoop;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync();

            string input = "";

            while (!killpending)
            {
               
                input = Console.ReadLine();
                worker.CancelAsync();
                Console.WriteLine("\nProductor pausado, toque cualquier tecla para reiniciar...");
                Console.ReadKey();
                worker.RunWorkerAsync();
                


                if (input.ToLower() == "kill")
                {
                    worker.CancelAsync();
                    break;
                }
            }

        }

        public static void KillProcess()
        {
            killpending = true;
        }

        public static void StopWorker()
        {
            Console.WriteLine("Señal de detencion recibida, se cancelará la operacion cuando termine el proceso actual.");
            worker.CancelAsync();
        }

        public static void InitWorker()
        {
            Console.WriteLine("Señal de inicio recibida. Iniciando...");
            worker.RunWorkerAsync();
        }

        public static async void MainLoop(object sender, DoWorkEventArgs args)
        {
            HttpClient client = new HttpClient();

            Random rng = new Random();

            List<Bulto> bultos;

            while (!worker.CancellationPending)
            {
                Console.WriteLine("Generando bulto...");
                var bulto = new Bulto()
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Bulto",
                    Peso = rng.Next(5, 30)
                };

                bultos = new List<Bulto>();
                bultos.Add(bulto);
                

                Console.WriteLine("Convirtiendo bulto...");
                var content = JsonConvert.SerializeObject(bultos);

                var stringContent = new StringContent(content, UnicodeEncoding.UTF8, "application/json");


                string responseString = "";

                while (true)
                {
                    try
                    {
                        Console.WriteLine("Intentando enviar...");
                        var response = await client.PostAsync("https://localhost:5021/api/Cinta/PonerBulto", stringContent);

                        Console.WriteLine("Bulto enviado, esperando respuesta...");
                        responseString = await response.Content.ReadAsStringAsync();
                        break;
                    }
                    catch (Exception)
                    {
                        await Task.Delay(1000);
                        Console.WriteLine("No se pudo enviar, reintentando...");                      
                    }
                }
                
               
                Console.WriteLine($"Respuesta: {responseString}");

                Console.WriteLine("Bulto ingresado, esperando...");
                Thread.Sleep(3000);
            }

            Console.WriteLine("Worker cancelado con exito.");
        }
    }
}