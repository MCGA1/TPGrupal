using CommonServices.Entities;
using CommonServices.Entities.Enum;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Managent
{
	public class BrazoService : BaseBalancerService
	{
		ILogger<BrazoService> _logger;

		string _url;

		public BrazoService(ILogger<BrazoService> logger, string url):base(logger)
		{
			_logger = logger;
			_url=url;
		}

		public override async Task<APIConfiguration> GetConfiguration()
		{
			_logger.LogInformation("Get configuration from request");

			using var client = new HttpClient();

			client.BaseAddress = new Uri(_url);

			var result = await client.GetAsync("api/configuration");

			result.EnsureSuccessStatusCode();

			string responseBody = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<APIConfiguration>(responseBody);
		}

		public override async Task<ServiceStatus> GetStatusRequest()
		{
			_logger.LogInformation("Get status request");

			using var client = new HttpClient();

			client.BaseAddress = new Uri(_url);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var result = await client.GetAsync("api/status");

			if(result.IsSuccessStatusCode)
				return ServiceStatus.Running;
			else if(result.StatusCode == System.Net.HttpStatusCode.Locked)
				return ServiceStatus.Stopped;
			else result.EnsureSuccessStatusCode();

			return ServiceStatus.Unknown;
		}

		public override async Task SendStatusRequest(ServiceStatus status)
		{
			_logger.LogInformation("Set status request");

			using var client = new HttpClient();

			client.BaseAddress = new Uri(_url);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

			var result = await client.PostAsync("api/status?value=1", content);

			result.EnsureSuccessStatusCode();
		}

		public override async Task UpdateConfiguration(APIConfiguration item)
		{
			_logger.LogInformation("Update configuration request");

			using var client = new HttpClient();

			client.BaseAddress = new Uri(_url);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var json = JsonConvert.SerializeObject(item);

			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var result = await client.PutAsync("api/configuration", content);

			result.EnsureSuccessStatusCode();
		}
	}
}