using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prensa.SensoresSystem
{
    public class Señal
    {
        public Señal(bool state)
        {
            State = state;
        }

        public bool State { get;}     
    }
}
