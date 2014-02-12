namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class DiscapacidadCargoMap: ClassMap<DiscapacidadCargo>
    {
        public DiscapacidadCargoMap()
        {
            Id(m => m.IdeDiscapacidadCargo, "IDEDISCAPACARGO")
                .GeneratedBy
                .Sequence("IDEDISCAPACARGO_SQ");
            References(x => x.Cargo, "IDECARGO");
            Map(x => x.TipoDiscapacidad, "TIPDISCAPA");
            Map(x => x.DescripcionDiscapacidad, "DESDISCAPA");
            Map(x => x.PuntajeDiscapacidad, "PUNTDISCAPA");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Map(x => x.DescripcionTipoDiscapacidad).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoDiscapacidad + " AND DG.VALOR = TIPDISCAPA AND DG.ESTACTIVO = 'A' )");

            Table("DISCAPACIDAD_CARGO");
        }
    }
}



      