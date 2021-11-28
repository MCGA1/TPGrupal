using CommonDomain;
using System.Threading.Tasks;

namespace Brazo.Core.Management
{
	public interface IPrensaManagement
	{
		Task SendPackage(Bulto package);
	}
}