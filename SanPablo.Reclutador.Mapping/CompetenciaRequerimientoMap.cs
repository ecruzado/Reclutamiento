
namespace SanPablo.Reclutador.Mapping
{
    //class CompetenciaRequerimientoMap
    //{
    //}

    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class CompetenciaRequerimientoMap : ClassMap<CompetenciaRequerimiento>
    {
        public CompetenciaRequerimientoMap()
        {
            Id(x => x.IdeCompetenciaRequerimiento, "IDECOMPETENCIASOLREQ")
              .GeneratedBy
              .Sequence("IDEOFRECEMOSSOLREQ_SQ");
            References(x => x.SolicitudRequerimiento, "IDESOLREQPERSONAL");
            Map(x => x.TipoCompetencia, "TIPCOMPETEN");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Map(x => x.DescripcionCompetencia).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoCompetencia + " AND DG.VALOR = TIPCOMPETEN AND DG.ESTACTIVO = 'A' )");

            Table("COMPETENCIAS_SOLREQ");

        }
    }

}
