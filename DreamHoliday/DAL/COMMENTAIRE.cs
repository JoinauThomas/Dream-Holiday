//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DreamHoliday.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class COMMENTAIRE
    {
        public int idCommentaire { get; set; }
        public int idBien { get; set; }
        public int idMembre { get; set; }
        public string COMM_libelle { get; set; }
        public System.DateTime COMM_date { get; set; }
        public int idLocation { get; set; }
        public string MEM_identifiant { get; set; }
    
        public virtual LOCATION LOCATION { get; set; }
    }
}
