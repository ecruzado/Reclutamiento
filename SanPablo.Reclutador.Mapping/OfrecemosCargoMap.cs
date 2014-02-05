namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class OfrecemosCargoMap : ClassMap<OfrecemosCargo>
    {
        public OfrecemosCargoMap()
        {
            Id(x => x.IdeOfrecemosCargo, "IDECOMPETENCIACARGO")
              .GeneratedBy
              .Sequence("IDECOMPETENCIACARGO_SQ");
            References(x => x.Cargo, "IDECARGO");
            Map(x => x.TipoOfrecimiento, "TIPOFRECIMIENTO");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Map(x => x.DescripcionOfrecimiento).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoOfrecimiento + " AND DG.VALOR = TIPOFRECIMIENTO AND DG.ESTACTIVO = 'A' )");

            Table("OFRECEMOS_CARGO");

        }
    }
}