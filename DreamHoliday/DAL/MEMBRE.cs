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
    
    public partial class MEMBRE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MEMBRE()
        {
            this.BIEN = new HashSet<BIEN>();
            this.LOCATION = new HashSet<LOCATION>();
        }
    
        public int idMembre { get; set; }
        public string MEM_mail { get; set; }
        public string MEM_nom { get; set; }
        public string MEM_prenom { get; set; }
        public string MEM_adresse { get; set; }
        public System.DateTime MEM_dateDeNaissance { get; set; }
        public string MEM_telephone { get; set; }
        public bool MEM_propriétaire { get; set; }
        public string MEM_Photo { get; set; }
        public string identifiant { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BIEN> BIEN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOCATION> LOCATION { get; set; }
    }
}
