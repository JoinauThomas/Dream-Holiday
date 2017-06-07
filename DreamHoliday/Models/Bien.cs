using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamHoliday.Models
{
    public class Bien
    {
        // le bien
        [Display(ResourceType = typeof(Resource), Name = "idBien")]
        public int idBien { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "pays")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public string pays { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ville")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public string ville { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "rue")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public string rue { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "numero")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public string numero { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "idMembre")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int idMembre { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "tarifNuit")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int tarifParNuit { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "tarifNettoyage")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int tarifNettoyage { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "noteMoyenne")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public double noteMoyenne { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "libelle")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public string libelle { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "photo")]
        public string photo { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "nbPersMax")]
        [Required(ErrorMessage ="aaaa")]
        public int nbPersonnesMax { get; set; }

        // les pieces

        [Display(ResourceType = typeof(Resource), Name = "sdb")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int salleDeBain { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "salon")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int salon { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "sam")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int salleAManger { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "toilette")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int toilette { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "cuisine")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int cuisine { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "chambre")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int chambre { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "dressing")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int dressing { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "veranda")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int veranda { get; set; }


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