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
            Map(x => x.CantidadExperiencia, "TIPCARGOTRABAJO");
            Map(x => x.PuntajeExperiencia, "CANTEXPERIENCIA");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Table("EXP_POSTULANTE");
        }

    }
}