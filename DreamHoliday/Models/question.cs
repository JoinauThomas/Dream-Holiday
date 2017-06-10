using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamHoliday.Models
{
    public class question
    {
        [Display(ResourceType = typeof(Resource), Name = "nom")]
        [Required(ErrorMessageResourceType = typeof(Resource),  ErrorMessageResourceName = "Error_nomRequis")]
        public string nom { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "prenom")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_PrenomRequis")]
        public string prenom { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "mail")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_mailRequis")]
        public string mail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "sujet")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_SujetRequis")]
        public string sujet { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "message")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_MessageRequis")]
        public string message { get; set; }

        public int idMembre { get; set; }

        public string identifiant { get; set; }

    }
}