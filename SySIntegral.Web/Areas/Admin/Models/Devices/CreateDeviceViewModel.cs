using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SySIntegral.Core.Entities.Assets;
using SySIntegral.Core.Entities.Organizations;

namespace SySIntegral.Web.Areas.Admin.Models.Devices
{
    public class CreateDeviceViewModel
    {
        public CreateDeviceViewModel()
        {
            Organizations = new List<Organization>();
            Assets = new List<Asset>();
        }

        public int Id { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "La Descripción del dispositivo es requerida")]
        public string Description { get; set; }

        [Display(Name = "ID de Dispositivo")]
        [Required(ErrorMessage = "El ID de Dispositivo es requerido")]
        public string UniqueId { get; set; }

        [Display(Name = "Organización")]
        [Required(ErrorMessage = "La Organización es requerida")]
        public int SelectedOrganizationId { get; set; }

        [Display(Name = "Instalación")]
        [Required(ErrorMessage = "La Instalación es requerida")]
        public int SelectedAssetId { get; set; }
        
        public bool IsLimitedByOrganization { get; set; }
        
        public List<Organization> Organizations { get; set; }  
        public List<Asset> Assets { get; set; }  
    }
}
