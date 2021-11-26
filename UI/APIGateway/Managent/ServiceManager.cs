using APIGateway.Contracts;
using APIGateway.Model.DTO;
using CommonServices.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace APIGateway.Managent
{
	public class ServiceManager : IServiceManager
	{
		ILogger<ServiceManager> _logger;

		LoadBalancer<CintaService> _cinta;
		LoadBalancer<BrazoService> _brazo;
		LoadBalancer<PrensaService> _prensa;

		public ServiceManager(ILogger<ServiceManager> logger, LoadBalancer<CintaService> cintaService, LoadBalancer<BrazoService> brazoService, LoadBalancer<PrensaService> prensaService)
		{
			_logger = logger;
			_cinta = cintaService;
			_brazo = brazoService;
			_prensa = prensaService;
		}

		public async Task AddServiceToBalancer(ServiceType type, string name, string url)
		{
			switch (type)
			{
				case ServiceType.Cinta:
					await _cinta.AddService(new CintaService(_logger, url, name));
					break;
				case ServiceType.Brazo:
					await _brazo.AddService(new BrazoService(_logger, url, name));
					break;
				case ServiceType.Prensa:
					await _prensa.AddService(new PrensaService(_logger, url, name));
					break;
				default:
					break;
			}
		}

		public async Task<dynamic> GetConfiguration(ServiceType type, string name)
		{
			var service = await GetServiceByTypeAndName(type, name);
			service.GetConfigurationRequest();
			throw new System.NotImplementedException();
		}

		public async Task<IAPIService> GetRunningService(ServiceType type) => type switch
		{
			ServiceType.Cinta => await _cinta.GetRunningService(),
			ServiceType.Brazo => await _brazo.GetRunningService(),
			ServiceType.Prensa => await _prensa.GetRunningService(),
			_ => null
		};

		public async Task<IList<APIServiceStatus>> GetStatusServices(ServiceType? type = null) => type switch
		{
			ServiceType.Cinta => await _cinta.GetStatusServices(),
			ServiceType.Brazo => await _brazo.GetStatusServices(),
			ServiceType.Prensa => await _prensa.GetStatusServices(),
			_ => await GetAllServices()
		};

		public Task<dynamic> SetConfiguration(ServiceType type, string name, APIConfiguration configurations)
		{
			throw new System.NotImplementedException();
		}

		private async Task<List<APIServiceStatus>> GetAllServices()
		{
			var cintaTask = _cinta.GetStatusServices();
			var brazoTask = _prensa.GetStatusServices();
			var prensaTask = _brazo.GetStatusServices();
			
			return (await cintaTask).Concat(await brazoTask).Concat(await prensaTask).ToList();
		}

		private async Task<IAPIService> GetServiceByTypeAndName(ServiceType type, string name) => type switch
		{
			ServiceType.Cinta => await _cinta.GetService(name),
			ServiceType.Brazo => await _brazo.GetService(name),
			ServiceType.Prensa => await _prensa.GetService(name),
			_ => null
		};
	}

	public enum ServiceType {
		Cinta,
		Brazo,
		Prensa
	}
}
