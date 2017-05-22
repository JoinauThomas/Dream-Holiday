using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamHoliday.Models
{
    public class Model_FormBigSearchBien
    {
        // le bien
        [Display(Name = "pays ou ville")]
        public string paysOuVille { get; set; }
        [Display(Name = "tarif par nuit")]
        public int tarifParNuit { get; set; }
        [Display(Name = "note moyenne")]
        public decimal noteMoyenne { get; set; }
        [Display(Name = "nbre de personnes max")]
        public int nbPersonnesMax { get; set; }

        // les pieces

        [Display(Name = "salle de bain")]
        public int salleDeBain { get; set; }
        public int toilette { get; set; }
        public int chambre { get; set; }

        // les options

        public bool bbq { get; set; }
        public bool piscine { get; set; }
        public bool jacuzzi { get; set; }
        public bool sauna { get; set; }
        public bool tv { get; set; }
        public bool teleDistribution { get; set; }
        public bool wifi { get; set; }
        public bool pingpong { get; set; }
        public bool tennis { get; set; }
        public bool transat { get; set; }
        [Display(Name = "cuisine équipée")]
        public bool cuisineEquipee { get; set; }
        [Display(Name = "machine à laver")]
        public bool machineALaver { get; set; }
        public bool jardin { get; set; }
        public bool parking { get; set; }
    }
}