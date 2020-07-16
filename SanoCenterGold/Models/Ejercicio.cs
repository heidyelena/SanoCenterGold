using SanoCenterGold.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanoCenterGold.Models
{
    public class Ejercicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DificultadEnum Dificultad { get; set; }
        public int Repeticiones { get; set; }
        public int Series { get; set; }
        public List<RetoEjercicio> Retos { get; set; }
     
    }
}
