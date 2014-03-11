

namespace SanPablo.Reclutador.Repository
{

    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    
    
    public class ConocimientoGeneralRequerimientoRepository : Repository<ConocimientoGeneralRequerimiento>, IConocimientoGeneralRequerimientoRepository
    {
        public ConocimientoGeneralRequerimientoRepository(ISession session)
            : base(session)
        {
        }
    }

}
