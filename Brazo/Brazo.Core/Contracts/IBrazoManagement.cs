using Brazo.Core.Model;
using CommonServices.Entities;
using CommonServices.Entities.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brazo.Core.Contracts
{
	public interface IBrazoManagement
	{
		Task<APIConfiguration> GetConfiguration();

		Task UpdateConfiguration(APIConfiguration config);

		Task<ServiceStatus> GetServiceStatus();

		Task SetServiceStatus(ServiceStatus status);

		Task<IList<ProcessedPackage>> GetProcessedPackages();
	}
}