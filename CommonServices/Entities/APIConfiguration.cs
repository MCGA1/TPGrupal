using CommonServices.Entities.Enum;

namespace CommonServices.Entities
{
	public class APIConfiguration
	{
		public int TiempoDeProcesamiento { get; set; }
		public ServiceStatus Estado { get; set; }
		public SensorConfiguration[] Sensores { get; set; }
	}
}