using Brazo.Core.Contracts;
using CommonDomain;
using CommonServices.Entities;
using CommonServices.Entities.Enum;
using System;
using System.Threading.Tasks;

namespace Brazo.Core.Management
{
	public class CintaManagement : ICintaManagement
	{
		

		public CintaManagement()
		{
			
		}


		public async Task<Bulto> GetPackage()
		{
			//RabbitMQ connection

			//keep connection open

			return new();
		}


		public async Task ProcessPackage()
		{
			//RabbitMQ connection

			//close connection
		}


	}
}
