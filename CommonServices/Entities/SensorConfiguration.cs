using CommonServices.Entities.Enum;

namespace CommonServices.Entities
{
	public class SensorConfiguration
	{
		public string Nombre { get; set; }
		public int TiempoDeProcesamiento { get; set; }
		public ServiceStatus Estado { get; set; }
	}
}