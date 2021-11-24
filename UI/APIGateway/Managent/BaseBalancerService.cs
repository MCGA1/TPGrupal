using APIGateway.Contracts;
using APIGateway.Exceptions;
using CommonServices.Entities;
using CommonServices.Entities.Enum;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace APIGateway.Managent
{
	public abstract class BaseBalancerService : IAPIService
	{
		public string Name { get; private set; }

		ServiceStatus _status;

		ILogger _logger;

		public BaseBalancerService(ILogger logger)
		{
			_logger = logger;
		}

		public abstract Task<APIConfiguration> GetConfiguration();

		public abstract Task UpdateConfiguration(APIConfiguration item);

		public abstract Task SendStatusRequest(ServiceStatus status);

		public abstract Task<ServiceStatus> GetStatusRequest();

		public async Task CheckStatus()
		{
			var status = await GetStatusRequest();
			if (status != _status)
			{
				_logger.LogInformation($"Service [{Name}] Status changed from [{_status}] to [{status}]");
				_status = status; 
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

	}
}