

namespace SanPablo.Reclutador.Repository
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


    public class TipoRequerimientoRepository : Repository<TipoRequerimiento>, ITipoRequerimiento
    {
        public TipoRequerimientoRepository(ISession session)
            : base(session)
        {
        }
    }
}
