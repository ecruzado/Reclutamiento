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

    }


}