using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SySIntegral.Web.Common.Filters;

namespace SySIntegral.Web.Areas.Api.Controllers
{
    [ApiController]
    [Route("api/contador-huevos")]
    [ServiceFilter(typeof(ApiAuthorizeFilter))]
    public class EggCounterController : ControllerBase
    {
       private readonly ILogger<EggCounterController> _logger;

        public EggCounterController(ILogger<EggCounterController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("status")]
        public IActionResult  GetStatus()
        {
            return Ok("active");
        }

        [HttpPost]
        [Route("registro")]
        public IActionResult  Register(EggCounterRegister data)
        {
            return Ok(data);
        }
    }

    public class EggCounterRegister
    {
        [JsonProperty("DispositivoID")]
        public string DeviceId { get; set; }

        [JsonProperty("CantidadHuevosBlancos")]
        public int? WhiteEggsCount { get; set; }

        [JsonProperty("CantidadHuevosColorados")]
        public int? BrownEggsCount { get; set; }
    }
}
