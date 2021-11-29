using APIGateway.Management;
using CommonServices.Entities.Enum;

namespace APIGateway.Model
{
	public class ServiceRequest
  {
    public ServiceType Type { get; set; }

    public string Name { get; set; }

    public string URL { get; set; }
  }
}