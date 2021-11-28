using Brazo.Core.Contracts;
using CommonDomain;
using CommonServices.Entities;
using CommonServices.Entities.Enum;
using System;
using System.Threading.Tasks;

namespace Brazo.Core.Management
{
	public class PrensaManagement : IPrensaManagement
	{
		

		public PrensaManagement()
		{
			
		}


		public async Task SendPackage(Bulto package)
		{
			//RabbitMQ connection

			
		}

	}
}
