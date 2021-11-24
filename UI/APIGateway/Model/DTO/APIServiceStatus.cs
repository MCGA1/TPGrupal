using CommonServices.Entities.Enum;

namespace APIGateway.Model.DTO
{
	public class APIServiceStatus
	{
		public string Name { get; set; }

		public ServiceStatus Status { get; set; }
	}
}
