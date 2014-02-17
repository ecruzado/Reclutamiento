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

    public class SedeRepository : Repository<Sede>, ISedeRepository
    {
        public SedeRepository(ISession session)
            : base(session)
        {
        }

        public IList<Sede> GetByTipSede()
        {
            var lista = GetBy(x => x.EstadoRegistro =="A");
            return lista;
        }
    }
}