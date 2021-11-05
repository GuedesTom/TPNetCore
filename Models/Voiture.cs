using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TPNetCore.Models
{
    public class Voiture
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Matricule { get; set; }
        public string Photo { get; set; }
        public int ProprietaireId { get; set; }
        public Proprietaire Proprietaires { get; set; }
    }
}
