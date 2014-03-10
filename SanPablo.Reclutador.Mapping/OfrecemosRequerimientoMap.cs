

namespace SanPablo.Reclutador.Mapping
{
    //class OfrecemosRequerimientoMap
    //{
    //}
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class OfrecemosRequerimientoMap : ClassMap<OfrecemosRequerimiento>
    {
        public OfrecemosRequerimientoMap()
        {
            Id(x => x.IdeOfrecemosRequerimiento, "IDEOFRECEMOSSOLREQ")
              .GeneratedBy
              .Sequence("IDEOFRECEMOSSOLREQ_SQ");
            References(x => x.SolicitudRequerimiento, "IDESOLREQPERSONAL");
            Map(x => x.TipoOfrecimiento, "TIPOFRECIMIENTO");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Map(x => x.DescripcionOfrecimiento).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoOfrecimiento + " AND DG.VALOR = TIPOFRECIMIENTO AND DG.ESTACTIVO = 'A' )");

            Table("OFRECEMOS_SOLREQ");

        }
    }

}
