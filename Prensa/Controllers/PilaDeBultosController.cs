using CommonServices.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prensa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PilaDeBultosController : ControllerBase
    {
        private readonly ILogger<PilaDeBultosController> _logger;

        public PilaDeBultosController(ILogger<PilaDeBultosController> logger)
        {
            _logger = logger;
        }

        [HttpPost("insertar bulto")]
        public void AgregarBulto([FromBody] BultoProcesado  bultoProcesado)
        {
            BultosStorageContext bultosStorageContext = new BultosStorageContext();
            bultosStorageContext.Add(bultoProcesado);
            bultosStorageContext.SaveChangesAsync();

        }

        [HttpPost]
        public bool Estado()
        {
            throw new NotImplementedException();
        }
    }
}
