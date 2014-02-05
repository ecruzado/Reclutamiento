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
            References(x => x.Cargo, "IDECARGO");
            Map(x => x.TipoCompetencia, "TIPCOMPETEN");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Table("CATEGORIA");

        }
    }
}