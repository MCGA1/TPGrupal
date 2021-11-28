using Prensa.SensoresSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonDomain;
using Serilog;
using NetMQ.Sockets;
using NetMQ;

namespace Prensa.PrensaSystem
{
    public static class MaquinaPrensado
    {
        static RequestSocket signaler;

        public delegate void ActiveSignalEventHandler(Señal signal);
        static event ActiveSignalEventHandler ActiveSensorSignal;

        public delegate void PassiveSignalEventHandler(bool state);
        static event PassiveSignalEventHandler PassiveSensorSignal;

        private static bool Estado
        {
            get
            {
                return Task.Run(async () => await SensorActivoCommunicator.GetStatus()).Result;
            }

            set
            {
                SensorActivoCommunicator.SendSignal(new Señal(value));
            }
        }

        private static bool Libre
        {
            get
            {
                return SensorPasivo.State;
            }

            set
            {
                SensorPasivo.State = value;
            }
        }

        static MaquinaPrensado()
        {
            ActiveSensorSignal += SensorActivoCommunicator.SendSignal;
            PassiveSensorSignal += SensorPasivo.SetState;
        }

        public static Bulto CurrentBulto { get; set; }
      
        public static async Task<BultoProcesado> Prensar(Bulto bulto)
        {
            PassiveSensorSignal(false);
            ActiveSensorSignal(new Señal(false));

            Log.Information("Prensa: Prensando...");
            await Task.Delay(1000).ConfigureAwait(false);

            Log.Information("Prensa: Bulto procesado, levantando prensa...");
            await Task.Delay(1000).ConfigureAwait(false);

            ActiveSensorSignal(new Señal(true));
            Log.Information("Prensa: Prensa levantada, moviendo bulto...");
            await Task.Delay(1000).ConfigureAwait(false);

            PassiveSensorSignal(true);


            return new BultoProcesado(bulto);

        }

        public static async Task StoreBulto(Bulto bulto)
        {

        }

        public static bool GetEstado()
        {
            return Estado && Libre ? true : false;
        }

       

    }
}
