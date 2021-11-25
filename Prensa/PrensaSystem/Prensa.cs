using Prensa.SensoresSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prensa.PrensaSystem
{
    public static class Prensa
    {       
        private static bool Estado 
        {
            get
            {
                return SensorActivo.State;
            }

            set
            {
                SensorActivo.State = value;
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

        public static Bulto CurrentBulto { get; set; }
      
        public static async void Prensar(Bulto bulto)
        {
            Libre = false;
            await Task.Delay(2000).ConfigureAwait(false);


            Libre = true;
            Estado = true;

        }

        public static bool GetEstado()
        {
            return Estado && Libre ? true : false;
        }

    }
}
