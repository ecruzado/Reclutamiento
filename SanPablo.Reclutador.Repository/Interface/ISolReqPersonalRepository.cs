

namespace SanPablo.Reclutador.Repository.Interface
{

    using SanPablo.Reclutador.Entity;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Data.OracleClient;
    using System.Configuration;
    using System.Collections;

    using System.Linq;
    using System.Transactions;

    public interface ISolReqPersonalRepository : IRepository<SolReqPersonal>
    {
        List<Cargo> GetTipCargo(int IdCargo);
        List<SolReqPersonal> GetListaSolReqPersonal(SolReqPersonal obj);
        int EliminaListaReemplazo(Reemplazo obj);
        int InsertTempReemplazo(Reemplazo obj);
        List<Reemplazo> GetListaReemplazo(Reemplazo obj);
        Int32 CreaSolicitudReemplazo(SolReqPersonal obj,Reemplazo objReemplazo);
        Int32 EnviaSolicitud(SolReqPersonal obj);
        List<SolReqPersonal> GetDatosSol(SolReqPersonal obj);
        void verificaPotenciales(ReclutamientoPersona obj);
        List<Reporte> GetListaReporteSeleccion(Reporte obj);
        List<Reporte> GetListaReporteResumen(Reporte obj);
        DataTable ListaReporteResumen(Reporte obj);

        DataTable ListaReporteSeleccion(Reporte obj);
        
        int EliminaSol(SolReqPersonal objSol);
        /// <summary>
        /// obtiene el responsable de relizar una accion determinada
        /// </summary>
        /// <param name="Tipo"></param>
        /// <param name="sede"></param>
        /// <param name="TipoReq"></param>
        /// <returns></returns>
        SolReqPersonal GetResponsable(string Tipo,Int32 sede,string TipoReq);

        /// <summary>
        /// actualiza el log de solicitud de requerimiento
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        void ActualizaLogSolReq(LogSolReqPersonal obj);


        List<CompetenciaRequerimiento> ListaCompetencias(int ideSolicitudReqPersonal);

        List<OfrecemosRequerimiento> ListaOfrecemos(int ideSolicitudReqPersonal);

        List<HorarioRequerimiento> ListaHorarios(int ideSolicitudReqPersonal);

        List<UbigeoReemplazo> ListaUbigeos(int ideSolicitudReqPersonal);

        List<CentroEstudioRequerimiento> ListaCentroEstudio(int ideSolicitudReqPersonal);

        List<NivelAcademicoRequerimiento> ListaNivelAcademico(int ideSolicitudReqPersonal);

        List<ConocimientoGeneralRequerimiento> ListaConocimientos(int ideSolicitudReqPersonal, string conocimiento);

        List<ExperienciaRequerimiento> ListaExperiencia(int ideSolicitudReqPersonal);

        List<DiscapacidadRequerimiento> ListaDiscapacidad(int ideSolicitudReqPersonal);

        List<EvaluacionRequerimiento> ListaEvaluacion(int ideSolicitudReqPersonal);

        int insertarSolicitudAmpliacion(SolReqPersonal solicitudAmpliacion, int ideUsuarioSuceso, int ideRolSuceso, string etapa, int idRolRespSgte, string indArea);

        int responsablePublicacion(int ideCargo, int ideSede);

    }
}


   