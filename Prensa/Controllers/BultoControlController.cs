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
    public class BultoControlController : ControllerBase
    {
        private readonly ILogger<BultoControlController> _logger;

        public BultoControlController(ILogger<BultoControlController> logger)
        {
            _logger = logger;
        }

        
    }
}