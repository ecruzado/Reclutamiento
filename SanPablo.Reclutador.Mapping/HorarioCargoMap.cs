namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class HorarioCargoMap : ClassMap<HorarioCargo>
    {
        public HorarioCargoMap()
        {
            Id(m => m.IdeHorarioCargo, "IDEHORARIOCARGO")
                .GeneratedBy
                .Sequence("IDEHORARIOCARGO_SQ");
            References(x => x.Cargo, "IDECARGO");
            Map(x => x.TipoHorario, "TIPHORARIO");
            Map(x => x.PuntajeHorario, "PUNTHORARIO");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Map(x => x.DescripcionHorario).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoHorario + " AND DG.VALOR = TIPHORARIO AND DG.ESTACTIVO = 'A' )");

            Table("HORARIO_CARGO");

        }
    }
}