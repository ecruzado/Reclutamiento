namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class ExperienciaCargoMap : ClassMap<ExperienciaCargo>
    {
        public ExperienciaCargoMap()
        {
            Id(m => m.IdeExperienciaCargo, "IDEEXPCARGO")
                .GeneratedBy
                .Sequence("IDEEXPCARGO_SQ");
            References(x => x.Cargo, "IDECARGO");
            Map(x => x.TipoExperiencia, "TIPEXPLABORAL ");
            Map(x => x.CantidadAnhosExperiencia, "CANTANHOEXP");
            Map(x => x.CantidadMesesExperiencia, "CANTMESESEXP");
            Map(x => x.PuntajeExperiencia, "PUNTEXPERIENCIA");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Map(x => x.DescripcionExperiencia).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoCargo + " AND DG.VALOR = TIPEXPLABORAL AND DG.ESTACTIVO = 'A' )");
            

            Table("EXPERIENCIA_CARGO");
        }

    }
}