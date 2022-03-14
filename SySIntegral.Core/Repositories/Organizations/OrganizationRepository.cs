using System;
using System.Collections.Generic;
using System.Text;
using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.Organizations;

namespace SySIntegral.Core.Repositories.Organizations
{
    public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(ApplicationDbContext context) : base(context)
        {

        }
    }

    public interface IOrganizationRepository : IRepository<Organization>
    {
    }
}
