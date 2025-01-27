namespace ProyectoWebMVC.Models
{
    public class Solicitud
    {
        public int IdSolicitud { get; set; }
        public required string Nombre { get; set; }
        public required string Cedula { get; set; }
        public int Edad {  get; set; }
        public required string LugarResidencia { get; set; }
        public int Ingresos { get; set; }
        public required string AmbienteFamiliar { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string Correo { get; set; }
    }
}
