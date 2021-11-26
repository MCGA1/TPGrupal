﻿using Prensa.SensoresSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonDomain;

namespace Prensa.PrensaSystem
{
    public static class MaquinaPrensado
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
      
        public static async Task<BultoProcesado> Prensar(Bulto bulto)
        {
            Libre = false;
            Estado = false;

            Console.WriteLine("Prensa: Prensando...");
            await Task.Delay(1000).ConfigureAwait(false);

            Console.WriteLine("Prensa: Bulto procesado, levantando prensa...");
            await Task.Delay(1000).ConfigureAwait(false);
        
            Console.WriteLine("Prensa: Prensa levantada, moviendo bulto...");
            await Task.Delay(1000).ConfigureAwait(false);

            Libre = true;
            Estado = true;

            Console.WriteLine("");
            return new BultoProcesado(bulto);

        }

        public static bool GetEstado()
        {
            return Estado && Libre ? true : false;
        }

    }
}
