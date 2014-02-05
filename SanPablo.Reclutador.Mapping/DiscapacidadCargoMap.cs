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

            Table("DISCAPACIDAD_CARGO");
        }
    }
}



      