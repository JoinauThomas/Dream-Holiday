using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamHoliday.Models
{
    public class Model_NewBien
    {
        public string PaysOuVille { get; set; }
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
        public bool cuisineEquipee { get; set; }
        public bool machineALaver { get; set; }

        public bool jardin { get; set; }
        public int salleDeBain { get; set; }
        public int salon { get; set; }
        public int salleAManger { get; set; }
        public int toilette { get; set; }
        public int cuisine { get; set; }
        public int chambre { get; set; }
        public int dressing { get; set; }
        public int veranda { get; set; }
    }
}