using System.ComponentModel.DataAnnotations;

namespace SySIntegral.Web.Areas.Admin.Models.Organizations
{
    public class CreateOrganizationViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }
        
    }
}
