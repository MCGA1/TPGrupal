using Brazo.Core.Contracts;
using Brazo.Core.Model;
using CommonDomain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brazo.Core.Management
{
	public class DataAccessService : IDataAccessService
	{
		private ILogger<DataAccessService> _logger;
		private IConfiguration _config;

		public DataAccessService(IConfiguration conf, ILogger<DataAccessService> logger)
		{
			_logger = logger;
			_config = conf;
		}

		public async Task<IList<ProcessedPackage>> GetProcessedPackages()
		{
			try
			{
				_logger.LogInformation("Getting all processed packages");
				var packages = new List<ProcessedPackage>();

				using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
				connection.Open();
				using var command = new SqlCommand("SELECT [creation_date] FROM [MCGA.TpGrupal].[dbo].[Brazo]", connection);

				using SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					DateTime dt = reader.GetDateTime(0);
					packages.Add(new ProcessedPackage() { Creation_Date = dt });
				}

				return packages;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error processing sava data");
				throw;
			}
		}

		public async Task SavePackage(Bulto package)
		{
			try
			{
				_logger.LogInformation("Saving processed package into db");

				using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
				connection.Open();
				using var command = new SqlCommand(
				 "INSERT INTO Brazo (Id, Name) " +
				 "VALUES (@Id, @Name)", connection);

				command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = package.Id;
				command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = package.Nombre;
				//command.Parameters.Add("@Creation_Date", SqlDbType.DateTime).Value = DateTime.Now;

				command.CommandType = CommandType.Text;
				command.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error processing sava data");
			}
		}
	}
}
