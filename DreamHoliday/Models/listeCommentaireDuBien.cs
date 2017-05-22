using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamHoliday.Models
{
    public class listeCommentaireDuBien
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_DateNaissRequis")]
        public int idCommentaire { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_DateNaissRequis")]
        public int idBien { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "commentaire")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_DateNaissRequis")]
        public string libelle { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "date")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_DateNaissRequis")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateMessage { get; set; }
    }
}