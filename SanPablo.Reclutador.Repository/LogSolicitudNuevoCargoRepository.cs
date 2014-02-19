namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;

    public class LogSolicitudNuevoCargoRepository : Repository<LogSolicitudNuevoCargo>, ILogSolicitudNuevoCargoRepository
    {
        public LogSolicitudNuevoCargoRepository(ISession session)
            : base(session)
        {
        }


    }
}
