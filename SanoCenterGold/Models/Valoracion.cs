using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanoCenterGold.Models
{
    public class Valoracion
    {
        public int Id { get; set; }
        public int Puntuacion { get; set; }
        public string Comentario { get; set; }
        public int IdReto { get; set; }
        public Reto Reto { get; set; }
        public int IdUsuario { get; set; } // igual aqui no se guarda el id del usurio porque este nombre es del atributo que se agrago al modelo de usuario.
        public Usuario Usuario { get; set; }
    }
}
