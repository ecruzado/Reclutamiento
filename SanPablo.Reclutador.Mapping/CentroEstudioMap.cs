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


            Table("CENTROEST_CARGO");
        }
    }
}
