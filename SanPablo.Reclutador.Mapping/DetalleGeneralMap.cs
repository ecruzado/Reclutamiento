namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class DetalleGeneralMap : ClassMap<DetalleGeneral>
    {
        public DetalleGeneralMap()
        {
            CompositeId()
                .KeyProperty(x => x.IdeGeneral, set => {
                    set.ColumnName("IDEGENERAL");
                })
                .KeyProperty(x => x.Valor, set =>
                {
                    set.ColumnName("VALOR");
                });
            References(x => x.General).Column("IDEGENERAL");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Map(x => x.Descripcion , "DESCRIPCION");
            Map(x => x.Referencia, "REFERENCIA");
            Table("DETALLE_GENERAL");


        }

    }
}
