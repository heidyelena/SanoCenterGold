using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanoCenterGold.Models
{
    public class Gimnasio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Localidad { get; set; }
        public int Telefono { get; set; }
        public List<Usuario>Usuarios { get; set; }
    }
}
