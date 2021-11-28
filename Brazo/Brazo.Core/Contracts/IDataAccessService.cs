using Brazo.Core.Model;
using CommonDomain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brazo.Core.Contracts
{
	public interface IDataAccessService
	{
		Task SavePackage(Bulto package);

		Task<IList<ProcessedPackage>> GetProcessedPackages();
	}
}