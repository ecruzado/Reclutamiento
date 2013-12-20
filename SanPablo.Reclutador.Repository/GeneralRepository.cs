namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class GeneralRepository : Repository<General>, IGeneralRepository
    {
        public GeneralRepository(ISession session)
            : base(session)
        { 
        }
     }
}