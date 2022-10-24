﻿using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Materias
{
    public class MateriaDTO
    {
        public int Id { get; set; }
        public int IdGrupo { get; set; }
        public string NombreGrupo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}
