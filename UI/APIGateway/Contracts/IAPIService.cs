using CommonServices.Entities;
using System.Threading.Tasks;

namespace APIGateway.Contracts
{
	public interface IAPIService
	{
		Task<APIConfiguration> GetConfiguration();

		Task UpdateConfiguration(APIConfiguration item);
	}
}