using CommonDomain;
using System.Threading.Tasks;

namespace Brazo.Core.Contracts
{
	public interface ICintaManagement
	{
		Task<Bulto> GetPackage();

		Task ProcessPackage();
	}
}