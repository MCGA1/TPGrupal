using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using CommonServices.Entities;
using CommonServices.Entities.Enum;
using Nancy.Metadata.Modules;
using Nancy.Swagger;
using Nancy.Swagger.Services;
using Swagger.ObjectModel;

namespace Brazo.API.Modules.Metadata
{
	public class ServiceMetadataModule : MetadataModule<PathItem>
	{
		public ServiceMetadataModule(ISwaggerModelCatalog modelCatalog)
		{
			modelCatalog.AddModels(
				typeof(APIConfiguration), typeof(ServiceStatus), typeof(SensorConfiguration)
			);

			Describe["GetServiceName"] = description => description.AsSwagger(
				with => with.Operation(
					op => op.OperationId("GetServiceName")
						.Tag("Service")
						.Summary("Get service Name")
						.Description("Get service name.")
						.Response((int)HttpStatusCode.OK, r => r.Description("OK"))
				)
			);

			Describe["GetConfiguration"] = description => description.AsSwagger(
				with => with.Operation(
					op => op.OperationId("GetConfiguration")
						.Tag("Service")
						.Summary("Get configuration")
						.Description("Get configuration.")
						.Response((int)HttpStatusCode.OK, r => r.Description("OK"))
				)
			);

			Describe["UpdateConfiguration"] = description => description.AsSwagger(
				with => with.Operation(
					op => op.OperationId("UpdateConfiguration")
						.Tag("Service")
						.Summary("Update configuration")
						.Description("Update configuration.")
						.Response((int)HttpStatusCode.OK, r => r.Description("OK"))
						.BodyParameter(p => p.Description("Material").Name("body").Schema<APIConfiguration>())
				)
			);

			Describe["GetStatus"] = description => description.AsSwagger(
				with => with.Operation(
					op => op.OperationId("GetStatus")
						.Tag("Service")
						.Summary("Get status")
						.Description("Get status.")
						.Response((int)HttpStatusCode.OK, r => r.Description("OK"))
				)
			);


			Describe["UpdateStatus"] = description => description.AsSwagger(
				with => with.Operation(
					op => op.OperationId("UpdateStatus")
						.Tag("Service")
						.Summary("Update status")
						.Parameter(p => p.Name("Value").Description("Identificador del estado").IsRequired().In(ParameterIn.Path))
						.Description("Update status.")
						.Response((int)HttpStatusCode.OK, r => r.Description("OK"))
				)
			);
		}

	}
}
