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

            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
        }

        public Usuario ValidarUsuario(string _correo, string _clave)
        {
            return Usuario.Where(u => u.Correo == _correo && u.Clave == _clave).FirstOrDefault();
        }
    }
}
