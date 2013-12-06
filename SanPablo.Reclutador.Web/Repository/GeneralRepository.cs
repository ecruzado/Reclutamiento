namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class GeneralRepository : Repository<General>, IGeneralRepository
    {
        public GeneralRepository(ISession session)
            : base(session)
        { 
        }
     }
}