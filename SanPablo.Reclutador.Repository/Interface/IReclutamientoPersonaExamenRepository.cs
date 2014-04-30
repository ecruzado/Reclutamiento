 

namespace SanPablo.Reclutador.Repository.Interface
{
   using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;
    using System.Data;
    
    public interface IReclutamientoPersonaExamenRepository : IRepository<ReclutamientoPersonaExamen>
    {
        /// <summary>
        /// obtine las evaluaciones del postulante
        /// </summary>
        /// <param name="idePostulante"></param>
        /// <param name="idReclutaPersona"></param>
        /// <param name="usuarioSession"></param>
        /// <returns></returns>
        List<ReclutamientoPersonaExamen> obtenerEvaluacionesPostulante(int idePostulante, int idReclutaPersona, string usuarioSession);

        /// <summary>
        /// Determina si acabo con las categorias y realiza la calificacion
        /// </summary>
        /// <param name="ideReclutaPerExamenCat"></param>
        /// <param name="idReclutaPersona"></param>
        /// <param name="usuarioSession"></param>
        void calificacionExamen(int ideReclutaPerExamenCat, int idReclutaPersona, string usuarioSession);

        /// <summary>
        /// Obtener la evaluacion del postulante
        /// </summary>
        /// <param name="idereclutaPersona"></param>
        /// <param name="ideReclutaPersonaExamen"></param>
        /// <returns></returns>
        DataSet ObtenerEvaluacionReporte(int idereclutaPersona, int ideReclutaPersonaExamen);

        /// <summary>
        /// Obtiene el examen del postulante para el formato pdf
        /// </summary>
        /// <param name="idereclutaPersona"></param>
        /// <param name="ideReclutaPersonaExamen"></param>
        /// <returns></returns>
        ResultadoExamen ObtenerEvaluacionReportePdf(int idereclutaPersona, int ideReclutaPersonaExamen);

        /// <summary>
        /// verifica si existe resultado de examen
        /// </summary>
        /// <param name="ideReclutaPersonaExamen"></param>
        /// <returns></returns>
        string existeResultado(int ideReclutaPersonaExamen);
    }

    
}
