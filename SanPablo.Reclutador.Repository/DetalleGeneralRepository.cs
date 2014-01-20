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

        public IList<DetalleGeneral> GetByTipoTabla(TipoTabla tipoTabla)
        {
            var lista = GetBy(x => x.IdeGeneral == (int)tipoTabla 
                && x.IndicadorActivo == IndicadorActivo.Activo);
            return lista;
        }

    }
}