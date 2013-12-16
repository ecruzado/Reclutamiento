namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class CriterioPorSubcategoriaMap : ClassMap<CriterioPorSubcategoria>
    {
        public CriterioPorSubcategoriaMap()
        {
            Id(m => m.CodigoCriterioPorSubcategoria, "IDECRITERIOXSUBCATEGORIA");
            References(x => x.Criterio).Column("IDECRITERIO");
            References(x => x.SubCategoria).Column("IDESUBCATEGORIA");
            Map(x => x.PuntajeMaximo, "PUNTAMAXIMO");
            Map(x => x.Prioridad, "PRIORIDAD");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            Table("CRITERIO_X_SUBCATEGORIA");
        }
    }
}