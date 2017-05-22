using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamHoliday.Models
{
    public class LocationBien
    {
        [Display(ResourceType = typeof(Resource), Name = "idBien")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int idBien { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "idMembre")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int idMembre { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateArrivee { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateDepart { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "tarifNuit")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int tarifNuit { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "tarifNettoyage")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public int tarifNettoyage { get; set; }


        public string prixSejour { get; set; }
        public string tarifAdmin { get; set; }
        public string prixtotal { get; set; }
        public int nbNuits { get; set; }

    }
}