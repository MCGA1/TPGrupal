using CommonServices.Entities;
using System.Threading.Tasks;

namespace APIGateway.Contracts
{
	public interface IAPIService
	{

		Task<APIConfiguration> GetConfigurationRequest();

		Task SetConfiguration(APIConfiguration item);

		string GetName();
	}
}