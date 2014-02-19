namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;
    using System.Linq.Expressions;

    public class LogSolicitudNuevoCargoRepository : Repository<LogSolicitudNuevoCargo>, ILogSolicitudNuevoCargoRepository
    {
        public LogSolicitudNuevoCargoRepository(ISession session)
            : base(session)
        {
        }
        public LogSolicitudNuevoCargo getMostRecentValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition)
        {
            var maxResultDate = QueryOver.Of<LogSolicitudNuevoCargo>()
                .Where(condition)
                .Select(Projections.Max<LogSolicitudNuevoCargo>(x => x.FechaSuceso));

            return _session.QueryOver<LogSolicitudNuevoCargo>()
                           .Where(condition)
                           .WithSubquery.WhereProperty(x => x.FechaSuceso).Eq(maxResultDate)
                            //or to get the "last" 2 rows...
                            //.OrderBy(x => x.ResultDate).Desc
                            //.Take(2)
                            //.List();
                            .SingleOrDefault();
        }

        public IList<LogSolicitudNuevoCargo> getTwoMostRecentValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition)
        {
            var maxResultDate = QueryOver.Of<LogSolicitudNuevoCargo>()
                .Where(condition)
                .Select(Projections.Max<LogSolicitudNuevoCargo>(x => x.FechaSuceso));

            return _session.QueryOver<LogSolicitudNuevoCargo>()
                           .Where(condition)
                           .WithSubquery.WhereProperty(x => x.FechaSuceso).Eq(maxResultDate)
                            //or to get the "last" 2 rows...
                           //.OrderBy(x => x.FechaSuceso).Desc
                           .Take(2)
                           .List();
                           // .SingleOrDefault();
        }

    }
}
