using System;
using System.Collections.Generic;
using System.Linq;
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

        public Organization GetByName(string name)
        {
            return GetAll().FirstOrDefault(x => x.Name == name);
        }
    }

    public interface IOrganizationRepository : IRepository<Organization>
    {
        Organization GetByName(string name);
    }
}
