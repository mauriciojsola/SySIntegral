﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SySIntegral.Core.Entities.EggsRegistry;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.EggsRegistry;
using SySIntegral.Web.Common.Filters;

namespace SySIntegral.Web.Areas.Api.Controllers
{
    [ApiController]
    [Route("api/contador-huevos")]
    [ServiceFilter(typeof(ApiAuthorizeFilter))]
    public class EggCounterController : ControllerBase
    {
       private readonly ILogger<EggCounterController> _logger;
       private readonly IRepository<EggRegistry> _eggRegistryRepository;

        public EggCounterController(ILogger<EggCounterController> logger,IRepository<EggRegistry> eggRegistryRepository)
        {
            _logger = logger;
            _eggRegistryRepository = eggRegistryRepository;
        }

        [HttpGet]
        [Route("status")]
        public IActionResult  GetStatus()
        {
            return Ok("active");
        }

        [HttpPost]
        [Route("registro")]
        public IActionResult  Register(EggCounterRegistry data)
        {
            if (data == null) return BadRequest("No data provided");
            if (string.IsNullOrEmpty(data.DeviceId)) return BadRequest("DispositivoID es requerido");
            if (!data.WhiteEggsCount.HasValue && !data.ColorEggsCount.HasValue) return BadRequest("La cantidad de al menos un tipo de color de huevos es requerida.");
            
            _eggRegistryRepository.Insert(new EggRegistry
            {
                DeviceId = data.DeviceId,
                Timestamp = DateTime.Now,
                WhiteEggsCount = data.WhiteEggsCount.GetValueOrDefault(0),
                ColorEggsCount = data.ColorEggsCount.GetValueOrDefault(0)
            });

            return Ok(data);
        }
    }

    public class EggCounterRegistry
    {
        [JsonProperty("DispositivoID")]
        public string DeviceId { get; set; }

        [JsonProperty("CantidadHuevosBlancos")]
        public int? WhiteEggsCount { get; set; }

        [JsonProperty("CantidadHuevosColor")]
        public int? ColorEggsCount { get; set; }
    }
}
