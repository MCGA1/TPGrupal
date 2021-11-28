using APIGateway.Contracts;
using APIGateway.Exceptions;
using APIGateway.Model.DTO;
using CommonServices.Entities;
using CommonServices.Entities.Enum;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIGateway.Management
{
	public abstract class BaseBalancerService : IAPIService
	{
		public string Name { get; private set; }

		ServiceStatus _status;

		ILogger _logger;

		public BaseBalancerService(ILogger logger, string name)
		{
			_logger = logger;
			Name = name;
		}

		public abstract Task<APIConfiguration> GetConfigurationRequest();

		public abstract Task UpdateConfigurationRequest(APIConfiguration item);

		public abstract Task SendStatusRequest(ServiceStatus status);

		public abstract Task<ServiceStatus> GetStatusRequest();

		public abstract Task<IEnumerable<PackageItem>> ListPackages();

		public async Task SetConfiguration(APIConfiguration item)
		{
			if (item.Estado != _status) await SetStatus(item.Estado);

			await UpdateConfigurationRequest(item);
		}

		public async Task CheckStatus()
		{
			try
			{
				var status = await GetStatusRequest();
				if (status != _status)
				{
					_logger.LogInformation($"Service [{Name}] Status changed from [{_status}] to [{status}]");
					_status = status;
				}
			}
			catch (Exception e)
			{
				_status = ServiceStatus.Failed;
			}
		}

		public async Task SetStatus(ServiceStatus status)
		{
			try
			{
				_logger.LogInformation($"Service [{Name}] Set service Status [{status}]");
				await SendStatusRequest(status);

				await CheckStatus();
			}
			catch (Exception e)
			{
				_logger.LogWarning(e, $"Service [{Name}] Unable to connect with service.");
				_status = ServiceStatus.Failed;
			}
		}

		public async Task<ServiceStatus> GetStatus()
		{
			await CheckStatus();

			return _status;
		}

		public async Task<IAPIService> GetService()
		{
			if (_status == ServiceStatus.Failed) throw new ServiceUnavailableException();
			return this;
		}

		public string GetName()
		{
			return Name;
		}

	}
}