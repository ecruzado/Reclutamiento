namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class CentroEstudioCargoMap: ClassMap<CentroEstudioCargo>
    {
        public CentroEstudioCargoMap()
        {
            Id(m => m.IdeCentroEstudioCargo, "IDECENTESTCARGO")
                .GeneratedBy
                .Sequence("IDECENTESTCARGO_SQ");
            References(x => x.Cargo, "IDECARGO");
            Map(x => x.TipoCentroEstudio, "TIPCENESTU");
            Map(x => x.TipoNombreCentroEstudio, "TIPNOMCENESTU");
            Map(x => x.PuntajeCentroEstudios, "PUNTACENTROEST");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Map(x => x.DescripcionTipoCentroEstudio).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoInstitucion + " AND DG.VALOR = TIPCENESTU)");
            Map(x => x.DescripcionNombreCentroEstudio).Formula("(select DG.DESCRIPCION END FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoInstitucion + " AND DG.VALOR = TIPNOMCENESTU)");
           
            Table("CENTROEST_CARGO");
        }
    }
}
