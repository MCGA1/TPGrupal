using Prensa.PrensaSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prensa.SensoresSystem
{
    public static class SensorPasivo
    {

        private static bool State { get; set; }

        private static bool Paused { get; set; }

        static SensorPasivo() 
        {
            MaquinaPrensado.PassiveSensorSignal += SetState;
        }

        public static bool IsPaused()
        {
            return Paused;
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
            Paused = status;
        }

    }
}
