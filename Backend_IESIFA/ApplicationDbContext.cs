﻿using Backend_IESIFA.Entities;
using Backend_IESIFA.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Backend_IESIFA
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new GeneroMap());
            modelBuilder.ApplyConfiguration(new NivelEducativoMap());
            modelBuilder.ApplyConfiguration(new GradoMap());
            modelBuilder.ApplyConfiguration(new GrupoMap());
            modelBuilder.ApplyConfiguration(new MateriaMap());
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<NivelEducativo> NivelesEducativos { get; set; }
        public DbSet<Grado> Grados { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Materia> Materias { get; set; }

    }
}
