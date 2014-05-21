namespace SanPablo.Reclutador.Repository.Interface
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Data.OracleClient;
    using System.Configuration;
    using System.Collections;

    using System.Linq;
    using System.Transactions;

    public interface IPostulanteRepository : IRepository<Postulante>
    {

        List<OportunidadLaboral> GetObtieneOpurtunidad(OportunidadLaboral obj);
        SolReqPersonal GetDatosSolGrupo(OportunidadLaboral obj);
        OportunidadLaboral ValidaPostulacion(OportunidadLaboral obj);

        List<OportunidadLaboral> GetCargosPublicados(OportunidadLaboral obj);

        void Postulacion(Postulante obj);
        List<OportunidadLaboral> GetMisPostulaciones(OportunidadLaboral obj);
        List<ReclutamientoPersona> GetPostulantesRanking(ReclutamientoPersona obj);
        List<ReclutamientoPersona> GetPostulantesPreseleccionado(ReclutamientoPersona obj);
        List<ReclutamientoPersona> GetPostulantesSeleccionados(ReclutamientoPersona obj);
        string ValidaSeleccion(ReclutamientoPersona obj);
        
        void UpdateEstadoPostulante(ReclutamientoPersona obj);

        DataTable getDataCvPostulante(Postulante obj);
        DataTable getDataCvNivelAcademico(Postulante obj);
        DataTable getDataCvExperiencias(Postulante obj);
        DataTable getDataCvConOfimatica(Postulante obj);
        DataTable getDataCvConIdiomas(Postulante obj);
        DataTable getDataCvConOtros(Postulante obj);
        DataTable getDataCvParientes(Postulante obj);
        DataTable getDataCvDiscapacidad(Postulante obj);

        /// <summary>
        /// lista Reporte de Postulantes BD
        /// </summary>
        /// <param name="postulante"></param>
        /// <returns></returns>
        List<PostulanteBDReporte> ListaPostulantesBDReporte(PostulanteBDReporte postulante);

        /// <summary>
        /// data Table  de postulantes BD
        /// </summary>
        /// <param name="postulante"></param>
        /// <returns></returns>
        DataTable DtPostulantesBDReporte(PostulanteBDReporte postulante);

        /// <summary>
        /// lista de Postulantes Potenciales
        /// </summary>
        /// <param name="postulante"></param>
        /// <returns></returns>
        List<ReportePostulantePotencial> ListaPostulantesPotenciales(ReportePostulantePotencial postulante);

        /// <summary>
        /// DataTable de postulantes potenciales 
        /// </summary>
        /// <param name="postulante"></param>
        /// <returns></returns>
        DataTable DtReportePostulantesPotencial(ReportePostulantePotencial postulante);


        List<CvPostulante> ListaCvPostulante(CvPostulante obj);

    }


}