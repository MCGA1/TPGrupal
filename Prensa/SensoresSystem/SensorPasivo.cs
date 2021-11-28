using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prensa.SensoresSystem
{
    public static class SensorPasivo
    {
        private static bool _state = true;
        public static bool State
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

        public static void SetState(bool state)
        {
            State = state;
        }

    }
}
