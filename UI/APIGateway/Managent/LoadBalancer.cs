using APIGateway.Contracts;
using APIGateway.Exceptions;
using APIGateway.Model.DTO;
using CommonServices.Entities.Enum;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Managent
{
	public class LoadBalancer<T> where T: BaseBalancerService
	{
		ILogger<LoadBalancer<T>> _logger;

		IList<T> _services;

		public LoadBalancer(IList<T> services, ILogger<LoadBalancer<T>> logger)
		{
			_services = services;
			_logger = logger;
		}

		public async Task AddService(T service)
		{
			_logger.LogInformation($"adding new service. Name [{service.Name}]");

			if (!_services.Any(x => x.Name == service.Name))
				_services.Add(service);
			else
				_logger.LogInformation($"The service already exists.");
		}

		public async Task<IList<APIServiceStatus>> GetStatusServices()
		{
			_logger.LogInformation("Getting status services.");
			return _services.Select(x => new APIServiceStatus() { Name = x.Name, Status = x.GetStatus().Result }).ToList();
		}

		public async Task<IAPIService> GetRunningService()
		{
			_logger.LogInformation("Get running services");

			Random r = new Random();

			var asyncStatusList = new List<(IAPIService, Task<ServiceStatus>)>();

			foreach (var item in _services)
			{
				asyncStatusList.Add((item, item.GetStatus()));
			}

			Task.WaitAll(asyncStatusList.Select(x => x.Item2).ToArray());

			var availableServices = asyncStatusList.Where(x => x.Item2.Result == ServiceStatus.Running).Select(x => x.Item1).ToList();

			//var availableServices = _services.Where(x => x.GetStatus().Result == ServiceStatus.Running).ToList();
			if (availableServices.Count() == 0) throw new NoAvailableServiceException();

			_logger.LogInformation($"Services count [{availableServices.Count}] take one using random value generator");

			var service = availableServices[r.Next(availableServices.Count())];

			_logger.LogInformation($"Service [{service.GetName()}] selected.");

			return service;
		}


		public async Task<IList<IAPIService>> GetServices()
		{
			_logger.LogInformation("Get services");

			Random r = new Random();
			var availableServices = _services.Where(x => x.GetStatus().Result != ServiceStatus.Failed).ToList();

			_logger.LogInformation($"Services count [{availableServices.Count}]");

			return availableServices.Select(x => x.GetService().Result).ToList();
		}

		public async Task SetStatus(string name, ServiceStatus status)
		{
			var service = _services.FirstOrDefault(x => x.Name == name);

			await service.SetStatus(status);
		}

		public async Task<T> GetService(string name)
		{
			return _services.FirstOrDefault(x => x.Name == name);
		}
	}
}
