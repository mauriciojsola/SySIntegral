using System;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SySIntegral.Core.Entities.Devices;
using SySIntegral.Core.Entities.EggsRegistry;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.CheckPoints;
using SySIntegral.Core.Repositories.Devices;
using SySIntegral.Core.Repositories.EggsRegistry;
using SySIntegral.Web.Common.Filters;

namespace SySIntegral.Web.Areas.Api.Controllers
{
    [ApiController]
    [Route("api/contador-huevos")]
    [ServiceFilter(typeof(ApiAuthorizeFilter))]
    [AllowAnonymous]
    public class EggCounterController : ControllerBase
    {
        private readonly ILogger<EggCounterController> _logger;
        private readonly IRepository<EggRegistry> _eggRegistryRepository;
        private readonly IInputDeviceRepository _inputDeviceRepository;
        private readonly ICheckPointRepository _checkPointRepository;

        public EggCounterController(ILogger<EggCounterController> logger, 
            IRepository<EggRegistry> eggRegistryRepository, 
            IInputDeviceRepository inputDeviceRepository,
            ICheckPointRepository checkPointRepository)
        {
            _logger = logger;
            _eggRegistryRepository = eggRegistryRepository;
            _inputDeviceRepository = inputDeviceRepository;
            _checkPointRepository = checkPointRepository;
        }

        [HttpGet]
        [Route("status")]
        public IActionResult GetStatus()
        {
            return Ok("active");
        }

        [HttpPost]
        [Route("registro")]
        public IActionResult Register(EggCounterRegistry data)
        {
            if (data == null) return BadRequest("No data provided");
            if (string.IsNullOrEmpty(data.DeviceId)) return BadRequest("DispositivoID es requerido");
            if (string.IsNullOrEmpty(data.ReadTimestamp)) return BadRequest("La fecha de lectura es requerida");
            //if (!data.WhiteEggsCount.HasValue && !data.ColorEggsCount.HasValue) return BadRequest("La cantidad de al menos un tipo de color de huevos es requerida.");

            var readTimestamp = ParseDate(data.ReadTimestamp);
            var exportTimestamp = ParseDate(data.ExportTimestamp);

            if (!readTimestamp.HasValue) return BadRequest($"La fecha de lectura tiene formato incorrecto. Debe ser YYYYMMDDHHMMSS.");

            var device = _inputDeviceRepository.GetByUniqueID(data.DeviceId);
            if (device == null) return BadRequest($"No existe un dispositivo registrado con ID {data.DeviceId}.");

            var reg = new EggRegistry
            {
                //OldDeviceId = data.DeviceId,
                Device = device,
                Timestamp = DateTime.Now,
                WhiteEggsCount = data.WhiteEggsCount.GetValueOrDefault(0),
                ColorEggsCount = data.ColorEggsCount.GetValueOrDefault(0),
                ReadTimestamp = readTimestamp,
                ExportTimestamp = exportTimestamp
            };
            _eggRegistryRepository.Insert(reg);

            var checkPoint = _checkPointRepository.GetByInputDevice(device.Id);
            if (checkPoint != null)
            {
                checkPoint.Countings.Add(new CheckPointCount
                {
                    Registry = reg,
                    EggRegistryId = reg.Id,
                    CheckPoint = checkPoint,
                    CheckPointId = checkPoint.Id
                });
                _checkPointRepository.Update(checkPoint);
            }
            
            return Ok(data);
        }

        private DateTime? ParseDate(string date)
        {
            try
            {
                return DateTime.ParseExact(date, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return default(DateTime?);
            }
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

        [JsonProperty("FechaLectura")]
        public string ReadTimestamp { get; set; }

        [JsonProperty("FechaExportacion")]
        public string ExportTimestamp { get; set; }
    }
}
