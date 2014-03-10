

namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Data;
    using System.Data.OracleClient;
    using System.Transactions;

    public class NivelAcademicoRequerimientoRepository : Repository<NivelAcademicoRequerimiento>, INivelAcademicoRequerimientoRepository
    {
        public NivelAcademicoRequerimientoRepository(ISession session)
            : base(session)
        {
        }
    }
}
