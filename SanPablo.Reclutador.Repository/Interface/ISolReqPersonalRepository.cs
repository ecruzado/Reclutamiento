

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

        List<CompetenciaReemplazo> ListaCompetencias(int ideSolicitudReqPersonal);

        List<OfrecemosReemplazo> ListaOfrecemos(int ideSolicitudReqPersonal);

        List<HorarioReemplazo> ListaHorarios(int ideSolicitudReqPersonal);

    }
}


   