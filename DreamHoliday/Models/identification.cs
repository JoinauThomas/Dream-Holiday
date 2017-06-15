using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamHoliday.Models
{
    public class identification
    {
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "Error_mailRequis")]
        [Display(ResourceType = typeof(Resource), Name = "idMembre")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_identifiantRequis")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_PasswordRequis")]
        public string Password { get; set; }
    }
}