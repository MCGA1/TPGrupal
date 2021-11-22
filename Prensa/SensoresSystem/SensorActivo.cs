using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prensa.SensoresSystem
{
    public static class SensorActivo
    {

        private static bool _state;
        public static bool State 
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                SendReadySignal();
            }
        } 

        public static void SendReadySignal()
        {
            // todo: Llamada a ZeroMQ
        }






    }
}
