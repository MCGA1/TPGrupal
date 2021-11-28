using APIGateway.Contracts;
using APIGateway.Management;
using APIGateway.Model.DTO;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PackageController : ControllerBase
	{
		IServiceManager _service;

		public PackageController(IServiceManager service)
		{
			_service = service;
		}

		[HttpGet]
		[Route("{type}")]
		public async Task<IEnumerable<PackageItem>> GetPackages(ServiceType type)
		{
			var service = await _service.GetRunningService(type);
			return await service.ListPackages();
		}
	}
}
