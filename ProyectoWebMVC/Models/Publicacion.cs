using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoWebMVC.Models
{
    public class Publicacion
    {
        
        public int Id { get; set; }

        public required string Raza { get; set; }
    
        public required string Sexo { get; set; }

        public int Edad { get; set; }

        public double Peso { get; set; }

        public string Foto { get; set; }
 
        [NotMapped]
        public IFormFile FotoArchivo { get; set; }

        public required string Tamaño { get; set; }

        public bool Esterilizacion { get; set; } 

        public required string Enfermedades { get; set; }

        public required string Comportamiento { get; set; }
    }
}
