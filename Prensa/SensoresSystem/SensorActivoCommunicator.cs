using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using Serilog;

namespace Prensa.SensoresSystem
{
    public static class SensorActivoCommunicator
    {
        static CancellationTokenSource cts = new CancellationTokenSource();

        static RequestSocket client;

        static SensorActivoCommunicator()
        {

        }


        public static async void SendSignal(Señal status)
        {
            try
            {
                Connect();
                client.SendFrame(status.State.ToString());
                Log.Information("Prensa: Señal de prensa enviada al SensorActivo. Estado: {0}", status.State);

                var response = await Task.Run(() => Convert.ToBoolean(client.ReceiveFrameString()));
                client.Close();
                if (response == true)
                    return;
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Log.Error($"Error en la comunicacion con el sensor activo. Metodo: SendSignal(status). Mensaje:{ex}");
                return;
            }

        }

        private static void Connect()
        {
            if (client != null)
                client.Close();
            client = new RequestSocket();

            client.Connect("tcp://localhost:5556");
        }


        public static async Task<bool> GetStatus()
        {
            bool response = false;

            while (true)
            {
                try
                {
                    Connect();

                    client.SendFrame("getstatus");
                    response = await Task.Run(() => Convert.ToBoolean(client.ReceiveFrameString()));

                    client.Close();

                    break;
                }
                catch (Exception ex)
                {
                    Log.Error($"Error en la comunicacion con el sensor activo. Metodo: GetStatus(status). Mensaje:{ex}");
                }

            }


            return response;

        }




    }
}
