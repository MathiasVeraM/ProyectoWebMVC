using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoWebMVC.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; } // Se utiliza como identificador único
        public string? Clave { get; set; }
        public int? Edad { get; set; } // Campo editable
        public string? Cedula { get; set; }
        public string? LugarResidencia { get; set; }
        public string? AmbienteFamiliar { get; set; }
        public string? FotoPerfilPath { get; set; } // Nombre de la imagen guardada en el servidor
        [NotMapped]
        public IFormFile? FotoPerfil { get; set; } // Para subir imágenes desde el formulario
    }
}
