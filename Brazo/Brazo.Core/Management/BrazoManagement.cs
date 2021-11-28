using Brazo.Core.Contracts;
using Brazo.Core.Model;
using CommonServices.Entities;
using CommonServices.Entities.Enum;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brazo.Core.Management
{
	public class BrazoManagement : IBrazoManagement
	{
		GlobalSystemInformation SystemInfo;
		IDataAccessService _dataAccess;

		public BrazoManagement(GlobalSystemInformation serviceManager, IDataAccessService dataAccess)
		{
			SystemInfo = serviceManager;
			_dataAccess = dataAccess;
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

		public async Task<IList<ProcessedPackage>> GetProcessedPackages()
		{
			return await _dataAccess.GetProcessedPackages();
		}
	}
}
