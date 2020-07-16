using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SanoCenterGold.Models
{
    public class Usuario: IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }        
        public string Foto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Dni { get; set; }
        public int Telefono { get; set; }
        public string Direccion { get; set; }
        public string Localidad { get; set; }
        public List<RetoUsuario> MisRetos { get; set; }
        public int IdGimnasio { get; set; }
        public Gimnasio Gimnasio { get; set; }

        public List<Valoracion> Valoraciones { get; set; }
        public List<Reto> RetosPropuestos { get; set; }
        
        public int RetosCompletados { get; set; }
    }
}
