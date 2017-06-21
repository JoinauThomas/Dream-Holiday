using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamHoliday.Models
{
    public class MesLocations
    {
        [Display(ResourceType = typeof(Resource), Name = "idLocation")]
        public int idLocation { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "dateEnregistrement")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateEnregistrement { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "idBien")]
        public int idBien { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "DateArrivees")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateArrivee { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "dateDeDepart")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateDepart { get; set; }


        [Display(ResourceType = typeof(Resource), Name = "nombreDeNuits")]
        public int nbNuits { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "adresse")]
        public string adresse { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "photo")]
        public string photo { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "estNote")]
        public bool estNote { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "estCommente")]
        public bool estCommente { get; set; }


    }
}