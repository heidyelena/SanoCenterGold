using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SanoCenterGold.Models
{
    public class RetoEjercicio
    {
        [Key]
        public int IdRetoEjercicio { get; set; }
        public int IdReto { get; set; }
        public Reto Reto { get; set; }
        public int IdEjercicio { get; set; }
        public Ejercicio Ejercicio { get; set; }
    }
}
