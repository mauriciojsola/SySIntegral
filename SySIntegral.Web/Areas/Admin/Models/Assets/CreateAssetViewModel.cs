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

        [Display(Name = "Organización")]
        [Required(ErrorMessage = "La Organización es requerida")]
        public int SelectedOrganizationId { get; set; }
        
        public bool IsLimitedByOrganization { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre de la Instalación es requerido")]
        public string Name { get; set; }

        public List<Organization> Organizations { get; set; }  
    }
}
