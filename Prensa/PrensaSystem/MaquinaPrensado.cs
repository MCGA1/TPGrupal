using Prensa.SensoresSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonDomain;
using Serilog;
using NetMQ.Sockets;
using NetMQ;
using System.Threading;
using CommonServices.Entities.Enum;

namespace Prensa.PrensaSystem
{
    public static class MaquinaPrensado
    {
        public delegate void ActiveSignalEventHandler(Señal signal);
        public static event ActiveSignalEventHandler ActiveSensorSignal;

        public  delegate void PassiveSignalEventHandler(bool state);
        public static event PassiveSignalEventHandler PassiveSensorSignal;

        const int DefaultSpeed = 2000;
        static int OverrideSpeed = 0;

        public static int CurrentSpeed()
        {
            if(OverrideSpeed != 0)
            {
                return OverrideSpeed;
            }
            else
            {
                return DefaultSpeed;
            }

        }

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
                return SensorPasivo.GetStatus();
            }

            set
            {
                SensorPasivo.SetState(value);
            }
        }

        static MaquinaPrensado()
        {
            ActiveSensorSignal += SensorActivoCommunicator.SendSignal;
            PassiveSensorSignal += SensorPasivo.SetState;
        }

        private static Bulto CurrentBulto { get; set; }
        private static BultoProcesado CurrentBultoProcesado { get; set; }

        public static async Task Prensar()
        {
            var speed = DefaultSpeed == 2000 ? DefaultSpeed : OverrideSpeed;
          
            PassiveSensorSignal(false);
            ActiveSensorSignal(new Señal(false));

            Log.Information("Prensa: Prensando...");
            Thread.Sleep(speed);
            

            Log.Information("Prensa: Bulto procesado, levantando prensa...");
            Thread.Sleep(500);

            Log.Information("Prensa: Prensa levantada, enviando señal al sensor activo...");
            ActiveSensorSignal(new Señal(true));

            PassiveSensorSignal(true);



            CurrentBultoProcesado = new BultoProcesado(CurrentBulto);
            CurrentBulto = null;

        }

        public static async Task<BultoProcesado> RemoveBulto()
        {
            var bultoprocesado = CurrentBultoProcesado;
            CurrentBultoProcesado = null;
            return bultoprocesado;
        }

        public static async Task StoreBulto(Bulto bulto)
        {
            CurrentBulto = bulto;
        }

        public static bool GetEstado()
        {
            return Estado && Libre ? true : false;
        }

       
        public static void SetSpeed(int milisSeconds)
        {
            OverrideSpeed = milisSeconds;
        }



    }
}
