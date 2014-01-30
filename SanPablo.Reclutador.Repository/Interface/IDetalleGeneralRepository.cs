namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;
    using System;

    public interface IDetalleGeneralRepository : IRepository<DetalleGeneral>
    {
        IList<DetalleGeneral> GetByTipoTabla(TipoTabla tipoTabla);

        IList<DetalleGeneral> GetByTableReference(TipoTabla tipoTabla, String referencia);
    }
}