namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class DepartamentoRepository : Repository<Departamento>,IDepartamentoRepository
    {
        public DepartamentoRepository(ISession session)
            : base(session)
        { 
        }
 
    }
}