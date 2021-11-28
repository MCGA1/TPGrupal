using APIGateway.Model.DTO;
using CommonServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIGateway.Contracts
{
	public interface IAPIService
	{

		Task<APIConfiguration> GetConfigurationRequest();

		Task SetConfiguration(APIConfiguration item);

		string GetName();
		Task<IEnumerable<PackageItem>> ListPackages();
	}
}