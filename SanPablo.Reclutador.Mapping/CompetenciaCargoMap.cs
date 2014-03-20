namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class CompetenciaCargoMap : ClassMap<CompetenciaCargo>
    {
        public CompetenciaCargoMap()
        {
            Id(x => x.IdeCompetenciaCargo, "IDECOMPETENCIACARGO")
              .GeneratedBy
              .Sequence("IDECOMPETENCIACARGO_SQ");
            References(x => x.Cargo,"IDECARGO");
            Map(x => x.TipoCompetencia, "TIPCOMPETEN");
            Map(x => x.Puntaje, "PUNTAJE");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Map(x => x.DescripcionCompetencia).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoCompetencia + " AND DG.VALOR = TIPCOMPETEN AND DG.ESTACTIVO = 'A' )");

            Table("COMPETENCIAS_CARGO");

        }
    }
}