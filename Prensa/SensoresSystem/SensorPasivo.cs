using Prensa.PrensaSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prensa.SensoresSystem
{
    public static class SensorPasivo
    {

        private static bool State { get; set; } = true;

        public static bool IsPaused { get; set; } = false;

        static SensorPasivo() 
        {
            MaquinaPrensado.PassiveSensorSignal += SetState;
        }


        public static void SetState(bool state)
        {
            State = state;
        }

        public static bool GetStatus()
        {
            return State;
        }

        public static void SetPaused(bool status)
        {
            IsPaused = status;
        }

    }
}
