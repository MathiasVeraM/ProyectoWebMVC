using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoWebMVC.Models;

namespace ProyectoWebMVC.Data
{
    public class ProyectoWebMVCContext : DbContext
    {
        public ProyectoWebMVCContext (DbContextOptions<ProyectoWebMVCContext> options)
            : base(options)
        {
        }
        public DbSet<ProyectoWebMVC.Models.Usuario> Usuario { get; set; }
        public DbSet<ProyectoWebMVC.Models.Solicitud> Solicitudes { get; set; }
        public DbSet<Publicacion> Publicaciones { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>(tb =>
            {
                tb.HasKey(col =>  col.Id);
                tb.Property(col => col.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.Nombre).HasMaxLength(50);
                tb.Property(col => col.Correo).HasMaxLength(50);
                tb.Property(col => col.Clave).HasMaxLength(25);
            });

            modelBuilder.Entity<Solicitud>(tb => {
                tb.HasKey(col => col.IdSolicitud);
                tb.Property(col => col.IdSolicitud)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.Nombre).HasMaxLength(50);
                tb.Property(col => col.Cedula).HasMaxLength(10);
                tb.Property(col => col.Edad).HasColumnType("int");
                tb.Property(col => col.LugarResidencia).HasMaxLength(100);
                tb.Property(col => col.Ingresos).HasColumnType("int");
                tb.Property(col => col.AmbienteFamiliar).HasMaxLength(200);
                tb.Property(col => col.Fecha).HasColumnType("DateTime");
            });

            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Solicitud>().ToTable("Solicitudes");
        }

        public Usuario ValidarUsuario(string _correo, string _clave)
        {
            return Usuario.Where(u => u.Correo == _correo && u.Clave == _clave).FirstOrDefault();
        }

    }
}
