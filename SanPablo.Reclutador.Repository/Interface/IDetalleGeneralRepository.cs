namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;
   

    public interface IDetalleGeneralRepository : IRepository<DetalleGeneral>
    {
        IList<DetalleGeneral> GetByTipoTabla(TipoTabla tipoTabla);

    }
}