using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamHoliday.Models
{
    public class Model_FormBigSearchBien
    {
        // le bien
        [Display(ResourceType = typeof(Resource), Name = "paysOuVille")]
        public string paysOuVille { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "tarifNuit")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_TarifNuitRequis")]
        public int tarifParNuit { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "noteMoyenne")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public decimal noteMoyenne { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "nbPersMax")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int nbPersonnesMax { get; set; }

        // les pieces

        [Display(ResourceType = typeof(Resource), Name = "sdb")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_champNecesaire")]
        public int salleDeBain { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "toilette")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_champNecesaire")]
        public int toilette { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "chambre")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_champNecesaire")]
        public int chambre { get; set; }

        // les options

        [Display(ResourceType = typeof(Resource), Name = "bbq")]
        public bool bbq { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "piscine")]
        public bool piscine { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "jacuzzi")]
        public bool jacuzzi { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "sauna")]
        public bool sauna { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "tv")]
        public bool tv { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "teledistrib")]
        public bool teleDistribution { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "wifi")]
        public bool wifi { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "pingpong")]
        public bool pingpong { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "tennis")]
        public bool tennis { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "transat")]
        public bool transat { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "cuisineEq")]
        public bool cuisineEquipee { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "machineALaver")]
        public bool machineALaver { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "jardin")]
        public bool jardin { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "parking")]
        public bool parking { get; set; }
    }
}