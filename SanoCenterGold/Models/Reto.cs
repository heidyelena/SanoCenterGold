using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SanoCenterGold.Models
{
    public class Reto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }

        [Display(Name = "Fecha Limite")]
        public DateTime FechaLimite { get; set; }
        public List<RetoEjercicio> Ejercicios { get; set; }
        public List<RetoUsuario> Usuarios { get; set; }

        [Display(Name = "Entrenador")]
        public int IdEntrenador { get; set; }
        public Usuario Entrenador { get; set; }

        public List<Valoracion> Valoraciones { get; set; }
    }
}
