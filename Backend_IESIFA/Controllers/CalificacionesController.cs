using AutoMapper;
using Backend_IESIFA.DTOs.Calificaciones;
using Backend_IESIFA.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_IESIFA.Controllers
{
    [ApiController]
    [Route("api/calificaciones")]
    public class CalificacionesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CalificacionesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }



        [HttpGet("calificacionParaAlta/{idMateria:int}")]
        public async Task<ActionResult<CalificacionAltaDTO>> CalificacionParaAlta(int idMateria)
        {

            var materia = await context.Materias
                .Include(x => x.Grupo)
                .Include(x => x.Grupo.Grado)
                .Include(x => x.Grupo.Grado.NivelEducativo)
                .FirstOrDefaultAsync(x => x.Id == idMateria);

            if (materia is null)
            {
                return NotFound($"La materia {idMateria} no existe.");
            }


            //Buscar periodo actual
            var periodoActual = await ObtenerPeriodoActualAsync(materia.Grupo.Grado.IdNivelEducativo);

            if (periodoActual is null)
            {
                return BadRequest("Por el momento, no hay un periodo activo.");
            }


            //Validar en que evaluacion se encuentra
            var evaluacion = await ObtenerEvaluacionAsync(periodoActual.Id, idMateria);

            if (evaluacion == -1)
            {
                return BadRequest("Esta materia ya ha sido evaluada.");
            }


            var alumnosGrupo = await context.Alumnos
                   .Where(x => x.IdGrupo == materia.IdGrupo)
                   .OrderBy(x => x.ApellidoPaterno)
                   .ThenBy(x => x.ApellidoMaterno)
                   .ThenBy(x => x.Nombre)
                   .ToListAsync();


            var califiacionRegistrar = alumnosGrupo.Select(x => new CalificacionAlumnoRegistrarDTO
            {
                IdAlumno = x.Id,
                Nombre = $"{x.ApellidoPaterno} {x.ApellidoMaterno} {x.Nombre}",
                NoCuenta = x.NoCuenta,
                Calificacion = 0
            }).ToList();


            return new CalificacionAltaDTO
            {
                IdMateria = materia.Id,
                NombreMateria = materia.Nombre,
                IdGrupo = materia.IdGrupo,
                NombreGrupo = materia.Grupo.Nombre,
                IdGrado = materia.Grupo.IdGrado,
                NombreGrado = materia.Grupo.Grado.Nombre,
                IdNivelEducativo = materia.Grupo.Grado.IdNivelEducativo,
                NombreNivelEducativo = materia.Grupo.Grado.NivelEducativo.Nombre,
                IdPeriodo = periodoActual.Id,
                NombrePeriodo = periodoActual.Nombre,
                Evaluacion = evaluacion,
                CalificacionesRegistrar = califiacionRegistrar
            };


        }



        [HttpGet("calificacionesDocenteSecundaria/{idMateria:int}")]
        public async Task<ActionResult<CalificacionesDocenteSecundariaDTO>> CalificacionesDocenteSecundaria(int idMateria)
        {

            var materia = await context.Materias
             .Include(x => x.Grupo)
             .Include(x => x.Grupo.Grado)
             .Include(x => x.Grupo.Grado.NivelEducativo)
             .FirstOrDefaultAsync(x => x.Id == idMateria);

            if (materia is null)
            {
                return NotFound($"La materia {idMateria} no existe.");
            }

            //Buscar periodo actual
            var periodoActual = await ObtenerPeriodoActualAsync(materia.Grupo.Grado.IdNivelEducativo);

            if (periodoActual is null)
            {
                return BadRequest("Por el momento, no hay un periodo activo.");
            }

            //Traer cabecera con periodo actual
            var cabeceraCalificaciones = await context.CalificacionCabeceras
                .FirstOrDefaultAsync(x => x.IdMateria == idMateria
                && x.IdPeriodo == periodoActual.Id);

            if (cabeceraCalificaciones is null)
            {
                return BadRequest($"No se encontraron calificaciones en el periodo: {periodoActual.Nombre}.");
            }


            //Validar en que evaluacion se encuentra
            var evaluacion = await ObtenerEvaluacionAsync(periodoActual.Id, idMateria);

            var listaCalificaciones = await context.CalificacionesDetalleSecundaria
                .Include(x => x.Alumno)
                .Where(x => x.IdCabecera == cabeceraCalificaciones.Id)
                .OrderBy(x => x.Alumno.ApellidoPaterno)
                .ThenBy(x => x.Alumno.ApellidoMaterno)
                .ThenBy(x => x.Alumno.Nombre)
                .ToListAsync();

            var calificaciones = listaCalificaciones.Select(x => new CalificacionSecundariaDTO
            {
                Nombre = $"{x.Alumno.ApellidoPaterno} {x.Alumno.ApellidoMaterno} {x.Alumno.Nombre}",
                NoCuenta = x.Alumno.NoCuenta,
                PrimeraEvaluacion = x.PrimeraEvaluacion,
                SegundaEvaluacion = x.SegundaEvaluacion,
                TerceraEvaluacion = x.TerceraEvaluacion,
                CuartaEvaluacion = x.CuartaEvaluacion,
                QuintaEvaluacion = x.QuintaEvaluacion
            }).ToList();

            return new CalificacionesDocenteSecundariaDTO
            {
                IdMateria = materia.Id,
                NombreMateria = materia.Nombre,
                IdGrupo = materia.IdGrupo,
                NombreGrupo = materia.Grupo.Nombre,
                IdGrado = materia.Grupo.IdGrado,
                NombreGrado = materia.Grupo.Grado.Nombre,
                IdNivelEducativo = materia.Grupo.Grado.IdNivelEducativo,
                NombreNivelEducativo = materia.Grupo.Grado.NivelEducativo.Nombre,
                IdPeriodo = periodoActual.Id,
                NombrePeriodo = periodoActual.Nombre,
                Evaluacion = evaluacion,
                Calificaciones = calificaciones
            };

        }

        [HttpGet("calificacionesDocentePreparatoria/{idMateria:int}")]
        public async Task<ActionResult<CalificacionesDocentePreparatoriaDTO>> CalificacionesDocentePreparatoria(int idMateria)
        {

            var materia = await context.Materias
             .Include(x => x.Grupo)
             .Include(x => x.Grupo.Grado)
             .Include(x => x.Grupo.Grado.NivelEducativo)
             .FirstOrDefaultAsync(x => x.Id == idMateria);

            if (materia is null)
            {
                return NotFound($"La materia {idMateria} no existe.");
            }

            //Buscar periodo actual
            var periodoActual = await ObtenerPeriodoActualAsync(materia.Grupo.Grado.IdNivelEducativo);

            if (periodoActual is null)
            {
                return BadRequest("Por el momento, no hay un periodo activo.");
            }

            //Traer cabecera con periodo actual
            var cabeceraCalificaciones = await context.CalificacionCabeceras
                .FirstOrDefaultAsync(x => x.IdMateria == idMateria
                && x.IdPeriodo == periodoActual.Id);

            if (cabeceraCalificaciones is null)
            {
                return BadRequest($"No se encontraron calificaciones en el periodo: {periodoActual.Nombre}.");
            }


            //Validar en que evaluacion se encuentra
            var evaluacion = await ObtenerEvaluacionAsync(periodoActual.Id, idMateria);

            var listaCalificaciones = await context.CalificacionesDetallePreparatoria
                .Include(x => x.Alumno)
                .Where(x => x.IdCabecera == cabeceraCalificaciones.Id)
                .OrderBy(x => x.Alumno.ApellidoPaterno)
                .ThenBy(x => x.Alumno.ApellidoMaterno)
                .ThenBy(x => x.Alumno.Nombre)
                .ToListAsync();

            var calificaciones = listaCalificaciones.Select(x => new CalificacionPreparatoriaDTO
            {
                Nombre = $"{x.Alumno.ApellidoPaterno} {x.Alumno.ApellidoMaterno} {x.Alumno.Nombre}",
                NoCuenta = x.Alumno.NoCuenta,
                PrimeraEvaluacion = x.PrimeraEvaluacion,
                SegundaEvaluacion = x.SegundaEvaluacion,
                TerceraEvaluacion = x.TerceraEvaluacion,
                CuartaEvaluacion = x.CuartaEvaluacion,
            }).ToList();

            return new CalificacionesDocentePreparatoriaDTO
            {
                IdMateria = materia.Id,
                NombreMateria = materia.Nombre,
                IdGrupo = materia.IdGrupo,
                NombreGrupo = materia.Grupo.Nombre,
                IdGrado = materia.Grupo.IdGrado,
                NombreGrado = materia.Grupo.Grado.Nombre,
                IdNivelEducativo = materia.Grupo.Grado.IdNivelEducativo,
                NombreNivelEducativo = materia.Grupo.Grado.NivelEducativo.Nombre,
                IdPeriodo = periodoActual.Id,
                NombrePeriodo = periodoActual.Nombre,
                Evaluacion = evaluacion,
                Calificaciones = calificaciones
            };

        }



        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] CalificacionesCreacionDTO calificacionesDTO)
        {

            var existeCalificacion = await ValidarCalificacionRegistradaAsync(calificacionesDTO.IdMateria
                , calificacionesDTO.IdPeriodo, calificacionesDTO.Evaluacion);

            if (existeCalificacion)
            {
                return BadRequest("Yas has registrado calificaciones de esta evaluación.");
            }

            //Almacenar calificaciones 'secundaria'
            if (calificacionesDTO.Evaluacion >= 1 && calificacionesDTO.Evaluacion <= 5 && calificacionesDTO.IdNivelEducativo == 1)
            {

                //Cuando es nuevo registro 
                if (calificacionesDTO.Evaluacion == 1)
                {
                    var calificacionCabecera = mapper.Map<CalificacionCabecera>(calificacionesDTO);
                    calificacionCabecera.UltimaModificacion = DateTime.Now;

                    await context.AddAsync(calificacionCabecera);

                    await context.SaveChangesAsync();

                    var idCabecera = calificacionCabecera.Id;

                    foreach (var item in calificacionesDTO.Detalles)
                    {
                        CalificacionDetalleSecundaria calificacionDetalleSecundaria = new()
                        {
                            IdCabecera = idCabecera,
                            IdAlumno = item.IdAlumno,
                            PrimeraEvaluacion = item.Calificacion,
                            SegundaEvaluacion = 0,
                            TerceraEvaluacion = 0,
                            CuartaEvaluacion = 0,
                            QuintaEvaluacion = 0
                        };

                        await context.AddAsync(calificacionDetalleSecundaria);
                    }

                    await context.SaveChangesAsync();

                    return NoContent();
                }

                //Segunda evaluación
                else if (calificacionesDTO.Evaluacion == 2)
                {
                    var cabeceraCalificacion = await context.CalificacionCabeceras
                        .FirstOrDefaultAsync(x => x.IdMateria == calificacionesDTO.IdMateria
                        && x.IdPeriodo == calificacionesDTO.IdPeriodo
                        && x.Evaluacion == 1);

                    if (cabeceraCalificacion is null)
                    {
                        return BadRequest("No existe registro de calificaciones de la primera evaluación.");
                    }

                    //Actualizo a segunda evaluación
                    cabeceraCalificacion.Evaluacion = 2;
                    cabeceraCalificacion.UltimaModificacion = DateTime.Now;

                    await context.SaveChangesAsync();

                    foreach (var item in calificacionesDTO.Detalles)
                    {
                        //obtengo las calificaciones del alumno para editar
                        var alumnoCalificaciones = await context.CalificacionesDetalleSecundaria
                            .FirstOrDefaultAsync(x => x.IdCabecera == cabeceraCalificacion.Id
                            && x.IdAlumno == item.IdAlumno);

                        //Si se encontro
                        if (alumnoCalificaciones != null)
                        {
                            alumnoCalificaciones.SegundaEvaluacion = item.Calificacion;
                        }
                        else
                        {
                            //Si es nuevo alumno, se registran a partir del parcial que llevan
                            CalificacionDetalleSecundaria calificacionDetalleSecundaria = new()
                            {
                                IdCabecera = cabeceraCalificacion.Id,
                                IdAlumno = item.IdAlumno,
                                PrimeraEvaluacion = 0,
                                SegundaEvaluacion = item.Calificacion,
                                TerceraEvaluacion = 0,
                                CuartaEvaluacion = 0,
                                QuintaEvaluacion = 0
                            };

                            await context.AddAsync(calificacionDetalleSecundaria);
                        }
                    }

                    await context.SaveChangesAsync();

                    return NoContent();
                }

                //Tercera evaluación
                else if (calificacionesDTO.Evaluacion == 3)
                {
                    var cabeceraCalificacion = await context.CalificacionCabeceras
                        .FirstOrDefaultAsync(x => x.IdMateria == calificacionesDTO.IdMateria
                        && x.IdPeriodo == calificacionesDTO.IdPeriodo
                        && x.Evaluacion == 2);

                    if (cabeceraCalificacion is null)
                    {
                        return BadRequest("No existe registro de calificaciones de la segunda evaluación.");
                    }

                    //Actualizo a tercera evaluación
                    cabeceraCalificacion.Evaluacion = 3;
                    cabeceraCalificacion.UltimaModificacion = DateTime.Now;

                    await context.SaveChangesAsync();

                    foreach (var item in calificacionesDTO.Detalles)
                    {
                        //obtengo las calificaciones del alumno para editar
                        var alumnoCalificaciones = await context.CalificacionesDetalleSecundaria
                            .FirstOrDefaultAsync(x => x.IdCabecera == cabeceraCalificacion.Id
                            && x.IdAlumno == item.IdAlumno);

                        //Si se encontro
                        if (alumnoCalificaciones != null)
                        {
                            alumnoCalificaciones.TerceraEvaluacion = item.Calificacion;
                        }
                        else
                        {
                            //Si es nuevo alumno, se registran a partir del parcial que llevan
                            CalificacionDetalleSecundaria calificacionDetalleSecundaria = new()
                            {
                                IdCabecera = cabeceraCalificacion.Id,
                                IdAlumno = item.IdAlumno,
                                PrimeraEvaluacion = 0,
                                SegundaEvaluacion = 0,
                                TerceraEvaluacion = item.Calificacion,
                                CuartaEvaluacion = 0,
                                QuintaEvaluacion = 0
                            };

                            await context.AddAsync(calificacionDetalleSecundaria);
                        }
                    }

                    await context.SaveChangesAsync();

                    return NoContent();
                }

                //Cuarta evaluación
                else if (calificacionesDTO.Evaluacion == 4)
                {
                    var cabeceraCalificacion = await context.CalificacionCabeceras
                        .FirstOrDefaultAsync(x => x.IdMateria == calificacionesDTO.IdMateria
                        && x.IdPeriodo == calificacionesDTO.IdPeriodo
                        && x.Evaluacion == 3);

                    if (cabeceraCalificacion is null)
                    {
                        return BadRequest("No existe registro de calificaciones de la tercera evaluación.");
                    }

                    //Actualizo a cuarta evaluación
                    cabeceraCalificacion.Evaluacion = 4;
                    cabeceraCalificacion.UltimaModificacion = DateTime.Now;

                    await context.SaveChangesAsync();

                    foreach (var item in calificacionesDTO.Detalles)
                    {
                        //obtengo las calificaciones del alumno para editar
                        var alumnoCalificaciones = await context.CalificacionesDetalleSecundaria
                            .FirstOrDefaultAsync(x => x.IdCabecera == cabeceraCalificacion.Id
                            && x.IdAlumno == item.IdAlumno);

                        //Si se encontro
                        if (alumnoCalificaciones != null)
                        {
                            alumnoCalificaciones.CuartaEvaluacion = item.Calificacion;
                        }
                        else
                        {
                            //Si es nuevo alumno, se registran a partir del parcial que llevan
                            CalificacionDetalleSecundaria calificacionDetalleSecundaria = new()
                            {
                                IdCabecera = cabeceraCalificacion.Id,
                                IdAlumno = item.IdAlumno,
                                PrimeraEvaluacion = 0,
                                SegundaEvaluacion = 0,
                                TerceraEvaluacion = 0,
                                CuartaEvaluacion = item.Calificacion,
                                QuintaEvaluacion = 0
                            };

                            await context.AddAsync(calificacionDetalleSecundaria);
                        }
                    }

                    await context.SaveChangesAsync();

                    return NoContent();
                }

                //Quinta evaluación
                else if (calificacionesDTO.Evaluacion == 5)
                {
                    var cabeceraCalificacion = await context.CalificacionCabeceras
                        .FirstOrDefaultAsync(x => x.IdMateria == calificacionesDTO.IdMateria
                        && x.IdPeriodo == calificacionesDTO.IdPeriodo
                        && x.Evaluacion == 4);

                    if (cabeceraCalificacion is null)
                    {
                        return BadRequest("No existe registro de calificaciones de la cuarta evaluación.");
                    }

                    //Actualizo a quinta evaluación
                    cabeceraCalificacion.Evaluacion = 5;
                    cabeceraCalificacion.UltimaModificacion = DateTime.Now;

                    await context.SaveChangesAsync();

                    foreach (var item in calificacionesDTO.Detalles)
                    {
                        //obtengo las calificaciones del alumno para editar
                        var alumnoCalificaciones = await context.CalificacionesDetalleSecundaria
                            .FirstOrDefaultAsync(x => x.IdCabecera == cabeceraCalificacion.Id
                            && x.IdAlumno == item.IdAlumno);

                        //Si se encontro
                        if (alumnoCalificaciones != null)
                        {
                            alumnoCalificaciones.QuintaEvaluacion = item.Calificacion;
                        }
                        else
                        {
                            //Si es nuevo alumno, se registran a partir del parcial que llevan
                            CalificacionDetalleSecundaria calificacionDetalleSecundaria = new()
                            {
                                IdCabecera = cabeceraCalificacion.Id,
                                IdAlumno = item.IdAlumno,
                                PrimeraEvaluacion = 0,
                                SegundaEvaluacion = 0,
                                TerceraEvaluacion = 0,
                                CuartaEvaluacion = 0,
                                QuintaEvaluacion = item.Calificacion
                            };

                            await context.AddAsync(calificacionDetalleSecundaria);
                        }
                    }

                    await context.SaveChangesAsync();

                    return NoContent();
                }

            }

            //Almacenar calificaciones 'preparatoria'
            if (calificacionesDTO.Evaluacion >= 1 && calificacionesDTO.Evaluacion <= 4 && calificacionesDTO.IdNivelEducativo == 2)
            {

                //Cuando es nuevo registro 
                if (calificacionesDTO.Evaluacion == 1)
                {
                    var calificacionCabecera = mapper.Map<CalificacionCabecera>(calificacionesDTO);
                    calificacionCabecera.UltimaModificacion = DateTime.Now;

                    await context.AddAsync(calificacionCabecera);

                    await context.SaveChangesAsync();

                    var idCabecera = calificacionCabecera.Id;

                    foreach (var item in calificacionesDTO.Detalles)
                    {
                        CalificacionDetallePreparatoria calificacionDetallePreparatoria = new()
                        {
                            IdCabecera = idCabecera,
                            IdAlumno = item.IdAlumno,
                            PrimeraEvaluacion = item.Calificacion,
                            SegundaEvaluacion = 0,
                            TerceraEvaluacion = 0,
                            CuartaEvaluacion = 0
                        };

                        await context.AddAsync(calificacionDetallePreparatoria);
                    }

                    await context.SaveChangesAsync();

                    return NoContent();
                }

                //Segunda evaluación
                else if (calificacionesDTO.Evaluacion == 2)
                {
                    var cabeceraCalificacion = await context.CalificacionCabeceras
                        .FirstOrDefaultAsync(x => x.IdMateria == calificacionesDTO.IdMateria
                        && x.IdPeriodo == calificacionesDTO.IdPeriodo
                        && x.Evaluacion == 1);

                    if (cabeceraCalificacion is null)
                    {
                        return BadRequest("No existe registro de calificaciones de la primera evaluación.");
                    }

                    //Actualizo a segunda evaluación
                    cabeceraCalificacion.Evaluacion = 2;
                    cabeceraCalificacion.UltimaModificacion = DateTime.Now;

                    await context.SaveChangesAsync();

                    foreach (var item in calificacionesDTO.Detalles)
                    {
                        //obtengo las calificaciones del alumno para editar
                        var alumnoCalificaciones = await context.CalificacionesDetallePreparatoria
                            .FirstOrDefaultAsync(x => x.IdCabecera == cabeceraCalificacion.Id
                            && x.IdAlumno == item.IdAlumno);

                        //Si se encontro
                        if (alumnoCalificaciones != null)
                        {
                            alumnoCalificaciones.SegundaEvaluacion = item.Calificacion;
                        }
                        else
                        {
                            //Si es nuevo alumno, se registran a partir del parcial que llevan
                            CalificacionDetallePreparatoria calificacionDetallePreparatoria = new()
                            {
                                IdCabecera = cabeceraCalificacion.Id,
                                IdAlumno = item.IdAlumno,
                                PrimeraEvaluacion = 0,
                                SegundaEvaluacion = item.Calificacion,
                                TerceraEvaluacion = 0,
                                CuartaEvaluacion = 0
                            };

                            await context.AddAsync(calificacionDetallePreparatoria);
                        }
                    }

                    await context.SaveChangesAsync();

                    return NoContent();
                }

                //Tercera evaluación
                else if (calificacionesDTO.Evaluacion == 3)
                {
                    var cabeceraCalificacion = await context.CalificacionCabeceras
                        .FirstOrDefaultAsync(x => x.IdMateria == calificacionesDTO.IdMateria
                        && x.IdPeriodo == calificacionesDTO.IdPeriodo
                        && x.Evaluacion == 2);

                    if (cabeceraCalificacion is null)
                    {
                        return BadRequest("No existe registro de calificaciones de la segunda evaluación.");
                    }

                    //Actualizo a tercera evaluación
                    cabeceraCalificacion.Evaluacion = 3;
                    cabeceraCalificacion.UltimaModificacion = DateTime.Now;

                    await context.SaveChangesAsync();

                    foreach (var item in calificacionesDTO.Detalles)
                    {
                        //obtengo las calificaciones del alumno para editar
                        var alumnoCalificaciones = await context.CalificacionesDetallePreparatoria
                            .FirstOrDefaultAsync(x => x.IdCabecera == cabeceraCalificacion.Id
                            && x.IdAlumno == item.IdAlumno);

                        //Si se encontro
                        if (alumnoCalificaciones != null)
                        {
                            alumnoCalificaciones.TerceraEvaluacion = item.Calificacion;
                        }
                        else
                        {
                            //Si es nuevo alumno, se registran a partir del parcial que llevan
                            CalificacionDetallePreparatoria calificacionDetallePreparatoria = new()
                            {
                                IdCabecera = cabeceraCalificacion.Id,
                                IdAlumno = item.IdAlumno,
                                PrimeraEvaluacion = 0,
                                SegundaEvaluacion = 0,
                                TerceraEvaluacion = item.Calificacion,
                                CuartaEvaluacion = 0
                            };

                            await context.AddAsync(calificacionDetallePreparatoria);
                        }
                    }

                    await context.SaveChangesAsync();

                    return NoContent();
                }

                //Cuarta Evluación
                else if (calificacionesDTO.Evaluacion == 4)
                {
                    var cabeceraCalificacion = await context.CalificacionCabeceras
                        .FirstOrDefaultAsync(x => x.IdMateria == calificacionesDTO.IdMateria
                        && x.IdPeriodo == calificacionesDTO.IdPeriodo
                        && x.Evaluacion == 3);

                    if (cabeceraCalificacion is null)
                    {
                        return BadRequest("No existe registro de calificaciones de la tercera evaluación.");
                    }

                    //Actualizo a cuarta evaluación
                    cabeceraCalificacion.Evaluacion = 4;
                    cabeceraCalificacion.UltimaModificacion = DateTime.Now;

                    await context.SaveChangesAsync();

                    foreach (var item in calificacionesDTO.Detalles)
                    {
                        //obtengo las calificaciones del alumno para editar
                        var alumnoCalificaciones = await context.CalificacionesDetallePreparatoria
                            .FirstOrDefaultAsync(x => x.IdCabecera == cabeceraCalificacion.Id
                            && x.IdAlumno == item.IdAlumno);

                        //Si se encontro
                        if (alumnoCalificaciones != null)
                        {
                            alumnoCalificaciones.CuartaEvaluacion = item.Calificacion;
                        }
                        else
                        {
                            //Si es nuevo alumno, se registran a partir del parcial que llevan
                            CalificacionDetallePreparatoria calificacionDetallePreparatoria = new()
                            {
                                IdCabecera = cabeceraCalificacion.Id,
                                IdAlumno = item.IdAlumno,
                                PrimeraEvaluacion = 0,
                                SegundaEvaluacion = 0,
                                TerceraEvaluacion = 0,
                                CuartaEvaluacion = item.Calificacion
                            };

                            await context.AddAsync(calificacionDetallePreparatoria);
                        }
                    }

                    await context.SaveChangesAsync();

                    return NoContent();
                }
            }

            return BadRequest("Error al guardar calificaciones.");
        }

        private async Task<bool> ValidarCalificacionRegistradaAsync(int idMateria, int idPeriodo, int evaluacion)
        {
            return await context.CalificacionCabeceras.AnyAsync(
                x => x.IdMateria == idMateria
                && x.IdPeriodo == idPeriodo
                && x.Evaluacion == evaluacion);
        }

        private async Task<Periodo> ObtenerPeriodoActualAsync(int idNivelAcademico)
        {
            DateTime hoy = DateTime.Now;

            return await context.Periodos.Where(x =>
            hoy >= x.FechaInicio && hoy <= x.FechaFin
            && x.IdNivelEducativo == idNivelAcademico)
                .FirstOrDefaultAsync();
        }

        private async Task<int> ObtenerEvaluacionAsync(int idPeriodo, int idMateria)
        {
            var calificacionCabecera = await context.CalificacionCabeceras
             .FirstOrDefaultAsync(x => x.IdPeriodo == idPeriodo
             && x.IdMateria == idMateria);

            if (calificacionCabecera is null)
            {
                //Aún no se ha creado, por lo tanto 'Evaluacion' será '1'
                return 1;
            }


            //Ver que tipo de evaluación será de acuerdo al nivel academico
            //'secundaria'
            if (calificacionCabecera.Evaluacion >= 0 && calificacionCabecera.Evaluacion <= 5 && calificacionCabecera.IdNivelEducativo == 1)
            {
                switch (calificacionCabecera.Evaluacion)
                {
                    case 1:
                        return 2;
                    case 2:
                        return 3;
                    case 3:
                        return 4;
                    case 4:
                        return 5;
                }
            }

            //'Preparatoria'
            if (calificacionCabecera.Evaluacion >= 0 && calificacionCabecera.Evaluacion <= 4 && calificacionCabecera.IdNivelEducativo == 2)
            {
                switch (calificacionCabecera.Evaluacion)
                {
                    case 1:
                        return 2;
                    case 2:
                        return 3;
                    case 3:
                        return 4;
                }
            }

            return -1;

        }


    }
}
