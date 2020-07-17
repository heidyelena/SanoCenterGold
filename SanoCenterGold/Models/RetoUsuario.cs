using SanoCenterGold.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SanoCenterGold.Models
{
    public class RetoUsuario
    {
        [Key]
        public int IdRetoUsuario { get; set; }
        public int IdReto { get; set; }
        public Reto Reto { get; set; }

        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public DateTime? FechaCompletado { get; set; }

        public EstadoRetoEnum EstadoDelReto { get; set; }
    }
}
