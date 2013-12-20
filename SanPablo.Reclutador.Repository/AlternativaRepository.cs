namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class AlternativaRepository : Repository<Alternativa>, IAlternativaRepository
    {
        public AlternativaRepository(ISession session)
            : base(session)
        {
        }
    }
}