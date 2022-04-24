using System;
using System.Collections.Generic;
using System.Text;
using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.Assets;
using SySIntegral.Core.Entities.Organizations;

namespace SySIntegral.Core.Repositories.Assets
{
    public class AssetRepository : GenericRepository<Asset>, IAssetRepository
    {
        public AssetRepository(ApplicationDbContext context) : base(context)
        {

        }
    }

    public interface IAssetRepository : IRepository<Asset>
    {
    }
}
