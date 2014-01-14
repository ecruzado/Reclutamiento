namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System.Collections.Generic;

    public class DetalleGeneralRepository : Repository<DetalleGeneral>, IDetalleGeneralRepository
    {
        public DetalleGeneralRepository(ISession session)
            : base(session)
        { 
        }

        public IList<DetalleGeneral> GetByTipoTabla(string tipoVal)
        {
            var lista = GetBy(x => x.TipoTabla == tipoVal);
            return lista;
        }

    }
}