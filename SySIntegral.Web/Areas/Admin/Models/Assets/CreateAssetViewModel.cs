using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SySIntegral.Core.Entities.Organizations;

namespace SySIntegral.Web.Areas.Admin.Models.Assets
{
    public class CreateAssetViewModel
    {
        public CreateAssetViewModel()
        {
            Organizations = new List<Organization>();
        }

        public int Id { get; set; }
        public int SelectedOrganizationId { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        public List<Organization> Organizations { get; set; }  
    }
}
