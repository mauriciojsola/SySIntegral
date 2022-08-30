using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SySIntegral.Core.Entities.EggsRegistry;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.Assets;
using SySIntegral.Core.Repositories.CheckPoints;
using SySIntegral.Core.Repositories.Reports;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    [Route("Admin/[Controller]")]
    [Area("Admin")]
    [Authorize]
    public partial class ReportsController : SySIntegralBaseController
    {
        private readonly ILogger<RegistriesController> _logger;
        private readonly IRepository<EggRegistry> _eggRegistryRepository;
        private readonly ICheckPointRepository _checkPointRepository;
        private readonly ICheckPointCountsReportRepository _checkPointCountsReportRepository;
        private readonly IAssetRepository _assetRepository;

        public ReportsController(ILogger<RegistriesController> logger,
            IRepository<EggRegistry> eggRegistryRepository,
            ICheckPointRepository checkPointRepository,
            ICheckPointCountsReportRepository checkPointCountsReportRepository,
            IAssetRepository assetRepository)
        {
            _logger = logger;
            _eggRegistryRepository = eggRegistryRepository;
            _checkPointRepository = checkPointRepository;
            _checkPointCountsReportRepository = checkPointCountsReportRepository;
            _assetRepository = assetRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

      
    }
}
