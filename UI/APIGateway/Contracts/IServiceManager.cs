using APIGateway.Managent;
using APIGateway.Model.DTO;
using CommonServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIGateway.Contracts
{
	public interface IServiceManager
	{
		Task AddServiceToBalancer(ServiceType type, string name, string url);
		Task<IList<APIServiceStatus>> GetStatusServices(ServiceType? type = null);
		Task<IAPIService> GetRunningService(ServiceType type);
		Task<APIConfiguration> GetConfiguration(ServiceType type, string name);
		Task SetConfiguration(ServiceType type, string name, APIConfiguration configurations);
	}
}