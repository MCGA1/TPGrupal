using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prensa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PilaDeBultosController : ControllerBase
    {
        private readonly ILogger<PilaDeBultosController> _logger;

        public PilaDeBultosController(ILogger<PilaDeBultosController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void AgregarBulto()
        {
            // Lo agrega en rabbit
            throw new NotImplementedException();
        }

        [HttpPost]
        public bool Estado()
        {
            throw new NotImplementedException();
        }
    }
}
