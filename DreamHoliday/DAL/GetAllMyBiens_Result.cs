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
    
    public partial class GetAllMyBiens_Result
    {
        public int idBien { get; set; }
        public string BIEN_Pays { get; set; }
        public string BIEN_ville { get; set; }
        public string BIEN_rue { get; set; }
        public string BIEN_numero { get; set; }
        public int idMembre { get; set; }
        public int BIEN_tarifParNuit { get; set; }
        public int BIEN_tarifNettoyage { get; set; }
        public Nullable<double> BIEN_noteMoyenne { get; set; }
        public string BIEN_libelle { get; set; }
        public string BIEN_photo { get; set; }
        public int BIEN_nbMaxPersonnes { get; set; }
    }
}
