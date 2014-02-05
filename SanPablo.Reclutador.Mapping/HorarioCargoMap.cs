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
            Map(x => x.EstadoACtivo, "ESTACTIVO");
            Table("CRITERIO");

        }
    }
}