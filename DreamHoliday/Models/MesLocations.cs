using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamHoliday.Models
{
    public class MesLocations
    {
        public int idLocation { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateEnregistrement { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "idBien")]
        public int idBien { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateArrivee { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateDepart { get; set; }

        public int nbNuits { get; set; }

        public string adresse { get; set; }

        public string photo { get; set; }

        public bool estNote { get; set; }

        public bool estCommente { get; set; }
        

    }
}