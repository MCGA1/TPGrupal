using Serilog;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices.Enricher
{
  public static class LoggingExtensions
  {
    public static LoggerConfiguration WithServiceName(
        this LoggerEnrichmentConfiguration enrich)
    {
      if (enrich == null)
        throw new ArgumentNullException(nameof(enrich));

      return enrich.With<ServiceNameEnricher>();
    }
  }
}
