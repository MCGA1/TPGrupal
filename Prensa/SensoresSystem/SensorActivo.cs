using NetMQ;
using NetMQ.Sockets;
using System;
using System.ComponentModel;

namespace Prensa.SensoresSystem
{
    public static class SensorActivoServer
    {

        static BackgroundWorker _sensorWorker;
        static RouterSocket server;
        static NetMQPoller _poller;

        private static bool _state = true;
        private static bool State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public static void Init()
        {
            _sensorWorker = new BackgroundWorker();
            _sensorWorker.DoWork += MainLoop;
            _sensorWorker.WorkerSupportsCancellation = true;
            _sensorWorker.RunWorkerAsync();
        }

        static SensorActivoServer()
        {

        }


        private static async void HandleIncomingMessage()
        {
            var msg = server.ReceiveFrameString();
            Console.WriteLine($"Mensaje recibido:{msg}");

            if (msg.ToLower() == "true" || msg.ToLower() == "false")
            {
                State = Convert.ToBoolean(msg);
                server.SendFrame("true");
            }

            else if (msg.ToLower() == "getstatus")
            {
                server.SendFrame(State.ToString());
            }

            else
            {
                server.SendFrame("null");
            }

        }


        private static async void MainLoop(object sender, DoWorkEventArgs args)
        {
            //server.ReceiveReady += HandleIncomingMessage;

            using (var server = new ResponseSocket("@tcp://localhost:5556"))
            {
                Console.WriteLine("SensorActivoServer: Servicio levantado, escuchando en localhost:5556");
                while (!_sensorWorker.CancellationPending)
                {
                    Console.WriteLine("Esperando mensaje...");
                    var msg = server.ReceiveFrameString();
                    Console.WriteLine($"Mensaje recibido:{msg}");

                    if (msg.ToLower() == "true" || msg.ToLower() == "false")
                    {
                        State = Convert.ToBoolean(msg);
                        server.SendFrame("true");
                    }

                    else if (msg.ToLower() == "getstatus")
                    {
                        server.SendFrame(State.ToString());
                    }

                    else
                    {
                        server.SendFrame("null");
                    }
                }
            }
        }


    }
}
