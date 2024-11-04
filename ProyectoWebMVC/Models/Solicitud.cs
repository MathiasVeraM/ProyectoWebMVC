namespace ProyectoWebMVC.Models
{
    public class Solicitud
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Cedula { get; set; }
        public int Edad {  get; set; }
        public required string LugarResidencia { get; set; }
        public int Ingresos { get; set; }
        public required string AmbienteFamiliar { get; set; }
    }
}
