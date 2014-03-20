namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using NHibernate.Criterion;
    using System;
   
    public class DetalleGeneralRepository : Repository<DetalleGeneral>, IDetalleGeneralRepository
    {
        public DetalleGeneralRepository(ISession session)
            : base(session)
        { 
        }

        public IList<DetalleGeneral> GetByTipoTabla(TipoTabla tipoTabla)
        {
            var lista = GetBy(x => x.IdeGeneral == (int)tipoTabla 
                && x.EstadoActivo == IndicadorActivo.Activo && x.Referencia == null);
            return lista;
        }

        public IList<DetalleGeneral> GetByTableReference(TipoTabla tipoTabla, String referencia)
        {
            var lista = GetBy(x => x.IdeGeneral == (int)tipoTabla
                           && x.EstadoActivo == IndicadorActivo.Activo && x.Referencia == referencia);
             
            return lista;
        }

        public string GetByTableDescription(TipoTabla tipoTabla, String valor)
        {
            var lista = GetSingle(x => x.IdeGeneral == (int)tipoTabla
                           && x.EstadoActivo == IndicadorActivo.Activo && x.Valor == valor);

            return lista.Descripcion;
        }

        

    }
}