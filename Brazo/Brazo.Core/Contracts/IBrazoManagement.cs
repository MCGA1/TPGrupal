using CommonServices.Entities;
using CommonServices.Entities.Enum;
using System.Threading.Tasks;

namespace Brazo.Core.Contracts
{
	public interface IBrazoManagement
	{
		Task<APIConfiguration> GetConfiguration();

		Task UpdateConfiguration(APIConfiguration config);

		Task<ServiceStatus> GetServiceStatus();

		Task SetServiceStatus(ServiceStatus status);

	}
}