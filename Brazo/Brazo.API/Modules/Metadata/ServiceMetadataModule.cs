using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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
			Describe["GetServiceName"] = description => description.AsSwagger(
				with => with.Operation(
					op => op.OperationId("GetServiceName")
						.Tag("Service")
						.Summary("Get service Name")
						.Description("Get service name.")
						.Response((int)HttpStatusCode.OK, r => r.Description("OK"))
				)
			);

		}

	}
}
