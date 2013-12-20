namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class EmpleadoRepository : Repository<Empleado> , IEmpleadoRepository
    {
        public EmpleadoRepository(ISession session)
            : base(session)
        { 
        }
    }
}