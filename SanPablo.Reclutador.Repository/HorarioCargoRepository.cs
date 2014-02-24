namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Linq.Expressions;

    public class HorarioCargoRepository : Repository<HorarioCargo>, IHorarioCargoRepository
    {
        public HorarioCargoRepository(ISession session)
            : base(session)
        { 
        }
        public HorarioCargo getMaxPuntValue(Expression<Func<HorarioCargo, bool>> condition)
        {
            var maxResultDate = QueryOver.Of<HorarioCargo>()
                .Where(condition)
                .Select(Projections.Max<HorarioCargo>(x => x.PuntajeHorario));

            return _session.QueryOver<HorarioCargo>()
                           .Where(condition)
                           .WithSubquery.WhereProperty(x => x.PuntajeHorario).Eq(maxResultDate)
                           .SingleOrDefault();
        }
     }
}