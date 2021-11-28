using CommonServices.Entities;
using CommonServices.Entities.Enum;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Management
{
	public class CintaService : BaseBalancerService
	{
		ILogger _logger;

		string _url;

		HttpClient _httpClient;

		public CintaService(ILogger logger, string url, string name) : base(logger, name)
		{
			_logger = logger;
			_url = url;

			_httpClient = new HttpClient();
			_httpClient.Timeout = new TimeSpan(0, 0, 3);
			_httpClient.BaseAddress = new Uri(_url);

			_httpClient.DefaultRequestHeaders.Accept.Clear();
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public override async Task<APIConfiguration> GetConfigurationRequest()
		{
			_logger.LogInformation("Get configuration from request");

			var result = await _httpClient.GetAsync("api/configuration");

			result.EnsureSuccessStatusCode();

			string responseBody = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<APIConfiguration>(responseBody);
		}

		public override async Task<ServiceStatus> GetStatusRequest()
		{
			_logger.LogInformation("Get status request");

			var result = await _httpClient.GetAsync("api/status");

			if (result.IsSuccessStatusCode)
				return ServiceStatus.Running;
			else if (result.StatusCode == System.Net.HttpStatusCode.Locked)
				return ServiceStatus.Stopped;
			else result.EnsureSuccessStatusCode();

			return ServiceStatus.Unknown;
		}

		public override async Task SendStatusRequest(ServiceStatus status)
		{
			_logger.LogInformation("Set status request");

			var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

			var result = await _httpClient.PostAsync($"api/status?type={status}", content);

			result.EnsureSuccessStatusCode();
		}

		public override async Task UpdateConfigurationRequest(APIConfiguration item)
		{
			_logger.LogInformation("Update configuration request");

			var json = JsonConvert.SerializeObject(item);

			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var result = await _httpClient.PutAsync("api/configuration", content);

			result.EnsureSuccessStatusCode();
		}
	}
}