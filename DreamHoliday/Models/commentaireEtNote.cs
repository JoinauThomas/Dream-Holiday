using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamHoliday.Models
{
    public class commentaireEtNote
    {
        public int idCommentaire { get; set; }
        public int idBien { get; set; }
        public int idMembre { get; set; }
        public string libelle { get; set; }
        public DateTime date { get; set; }
        public int idLocation { get; set; }
        public int note { get; set; }
    }
}