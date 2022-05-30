using System;
using System.Collections.Generic;
using System.Linq;
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

        public Asset GetByName(string name, int organizationId)
        {
            return GetAll().FirstOrDefault(x => x.Name == name && x.Organization.Id == organizationId);
        }

        public IQueryable<Asset> GetByOrganization(int organizationId)
        {
            return GetAll().Where(x => x.Organization.Id == organizationId);
        }
    }

    public interface IAssetRepository : IRepository<Asset>
    {
        Asset GetByName(string modelName, int modelSelectedOrganizationId);
        IQueryable<Asset> GetByOrganization(int organizationId);
    }
}
