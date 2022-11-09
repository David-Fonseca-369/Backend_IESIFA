using AutoMapper;
using Backend_IESIFA.DTOs.Grados;
using Backend_IESIFA.DTOs.Grupos;
using Backend_IESIFA.DTOs.Materias;
using Backend_IESIFA.DTOs.Usuarios;
using Backend_IESIFA.Entities;

namespace Backend_IESIFA.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            #region Grupo
            //Origen | destino
            CreateMap<Grupo, GrupoDTO>()
                //Esto mapea la prpiedad que contiene la clave externa
                .ForMember(x => x.NombreGrado, x => x.MapFrom(g => g.Grado.Nombre))
                .ForMember(x => x.NombreNivelEducativo, x => x.MapFrom(n => n.Grado.NivelEducativo.Nombre));


            CreateMap<GrupoCrearDTO, Grupo>();
            #endregion


            #region Grado
            CreateMap<Grado, GradoSelectorDTO>();
            #endregion

            #region Materia
            CreateMap<Materia, MateriaDTO>()
            .ForMember(x => x.NombreGrupo, x => x.MapFrom(g => g.Grupo.Nombre));

            CreateMap<Materia, MateriaEditarDTO>();

            CreateMap<MateriaCrearDTO, Materia>();

            #endregion

            #region Usuarios
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(x => x.NombreRol, x => x.MapFrom(r => r.Rol.Nombre));
            #endregion
        }
    }
}
