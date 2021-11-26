using System;

namespace CommonServices.Context
{
    public class BultoProcesado:IBultoProcesado
    {
        public Guid Id { get; }

        public int Peso { get; }

        public string Nombre { get; set; }
    }
}