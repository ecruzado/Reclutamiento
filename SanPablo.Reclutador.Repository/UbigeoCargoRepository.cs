namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class UbigeoCargoRepository : Repository<UbigeoCargo>, IUbigeoCargoRepository
    {
        public UbigeoCargoRepository(ISession session)
            : base(session)
        { 
        }
     }
}