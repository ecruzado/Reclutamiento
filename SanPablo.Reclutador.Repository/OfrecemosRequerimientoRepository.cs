

namespace SanPablo.Reclutador.Repository
{
   
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class OfrecemosRequerimientoRepository : Repository<OfrecemosRequerimiento>, IOfrecemosRequerimientoRepository
    {
        public OfrecemosRequerimientoRepository(ISession session)
            : base(session)
        {
        }
    }

}
