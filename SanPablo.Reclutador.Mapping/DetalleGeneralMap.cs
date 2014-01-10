namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class DetalleGeneralMap : ClassMap<DetalleGeneral>
    {
        public DetalleGeneralMap()
        {
            Id(m => m.IdeDetalleGeneral, "IDEDETALLEGENERAL");
            References(x => x.General).Column("IDEGENERAL");
            Map(x => x.TipoTabla , "TIPTABLA");
            Map(x => x.Valor, "VALOR");
            Map(x => x.Descripcion , "DESCRIPCION");
            Table("DETALLE_GENERAL");
        }

    }
}
