using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamHoliday.Models
{
    public class Message
    {
        public int idMessage { get; set; }
        public int idMembre { get; set; }
        [Display(ResourceType = typeof(Resource), Name ="nom")]
        public string nom { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "prenom")]
        public string prenom { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "mail")]
        public string mail { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "sujet")]
        public string sujet { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "message")]
        public string libelle { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateEnvoi { get; set; }
    }
}