using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanoCenterGold.Models
{
    public class UsuariosConRoles
    {
        public IEnumerable<Usuario> Entrenadores { get; set; }
        public IEnumerable<Usuario> Usuarios { get; set; }

    }
}
