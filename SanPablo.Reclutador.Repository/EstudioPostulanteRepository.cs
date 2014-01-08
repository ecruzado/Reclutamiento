namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class EstudioPostulanteRepository: Repository<EstudioPostulante>,IEstudioPostulanteRepository
    {
        public EstudioPostulanteRepository(ISession session)
            : base(session)
        {
        }
    }
}
