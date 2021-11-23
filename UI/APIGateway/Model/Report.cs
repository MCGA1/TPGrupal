using System;

namespace APIGateway.Controllers
{
	public class Report
	{
		public int Id { get; set; }

		public string Msg { get; set; }

		public string Severity { get; set; }

		public DateTime Timestamp { get; set; }

		public string Ex { get; set; }

		public int ThreadId { get; set; }

		public string SourceContext { get; set; }

		public string ServiceName { get; set; }

	}
}