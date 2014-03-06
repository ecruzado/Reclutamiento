

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
        string CreaSolicitudReemplazo(SolReqPersonal obj,Reemplazo objReemplazo);


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

        int insertarSolicitudAmpliacion(SolReqPersonal solicitudAmpliacion, int ideUsuarioSuceso, int ideRolSuceso, string etapa, string codRolRespSgte, string indArea);

    }
}


   