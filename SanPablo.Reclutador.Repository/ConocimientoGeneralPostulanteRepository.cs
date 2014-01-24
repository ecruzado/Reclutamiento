namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class ConocimientoGeneralPostulanteRepository : Repository<ConocimientoGeneralPostulante>, IConocimientoGeneralPostulanteRepository
    {
        public ConocimientoGeneralPostulanteRepository(ISession session)
            : base(session)
        {
        }
    }
}
