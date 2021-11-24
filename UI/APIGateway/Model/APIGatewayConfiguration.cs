namespace APIGateway.Model
{
	public class APIGatewayConfiguration
	{
		public ServiceInfo[] Cinta { get; set; }
		public ServiceInfo[] Brazo { get; set; }
		public ServiceInfo[] Prensa { get; set; }
	}

	public class ServiceInfo
	{
		public string Name { get; set; }
		public string URL { get; set; }

		public bool IsRunning { get; set; }
	}

}
