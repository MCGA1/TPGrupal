using APIGateway.Managent;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReportController : ControllerBase
	{
		public IConfiguration Config { get; set; }

		public ReportController(IConfiguration config)
		{
			Config = config;
		}

		[HttpGet]
		public async Task<IEnumerable<Report>> GetReport()
		{
			string sqlOrderDetails = "SELECT * FROM [EventLogging].[Logs];";

			using var connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));
			
			var list = connection.Query<Report>(sqlOrderDetails).ToList();
			
			return list;
		}
	}
}
