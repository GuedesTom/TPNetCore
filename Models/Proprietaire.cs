using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TPNetCore.Models
{
    public class Proprietaire
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Photo { get; set; }
        public ICollection<Voiture> Voitures { get; set; }
    }
}
