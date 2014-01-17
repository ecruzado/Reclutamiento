namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class CriterioMap : ClassMap<Criterio>
    {
        public CriterioMap()
        {
            Id(m => m.IdeCriterio, "IDECRITERIO")
                .GeneratedBy
                .Sequence("IDECRITERIO_SQ");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            Map(x => x.TipoMedicion, "TIPMEDICION");
            Map(x => x.TipoCriterio, "TIPCRITERIO");
            Map(x => x.TipoModo, "TIPMODO");
            Map(x => x.Pregunta, "PREGUNTA");
            Map(x => x.TipoCalificacion, "TIPCALIFICACION");
            Map(x => x.OrdenImpresion, "ORDENIMPRESION");
            //HasManyToMany(x => x.SubCategorias)
            //    .Cascade.All()
            //    .Inverse()
            //    .Table("CRITERIO_X_SUBCATEGORIA");
            Table("CRITERIO");
        }
    }
}