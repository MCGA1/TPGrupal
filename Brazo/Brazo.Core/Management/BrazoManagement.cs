using Brazo.Core.Contracts;
using CommonServices.Entities;
using CommonServices.Entities.Enum;
using System;
using System.Threading.Tasks;

namespace Brazo.Core.Management
{
	public class BrazoManagement : IBrazoManagement
	{
		GlobalSystemInformation SystemInfo;

		public BrazoManagement(GlobalSystemInformation serviceManager)
		{
			SystemInfo = serviceManager;
		}

		public async Task<APIConfiguration> GetConfiguration()
		{
			return SystemInfo.Config;
		}

		public async Task UpdateConfiguration(APIConfiguration config)
		{
			SystemInfo.Status = config.Estado;
			SystemInfo.Config = config;
		}

		public async Task<ServiceStatus> GetServiceStatus()
		{
			return SystemInfo.Status;
		}

		public async Task SetServiceStatus(ServiceStatus status)
		{
			SystemInfo.Status = status;
		}
	}
}
