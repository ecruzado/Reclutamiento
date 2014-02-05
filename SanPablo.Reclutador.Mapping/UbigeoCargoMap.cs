namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class UbigeoCargoMap : ClassMap<UbigeoCargo>
    {
        public UbigeoCargoMap()
        {
            Id(m => m.IdeUbigeoCargo, "IDEUBIGEOCARGO");
            References(x => x.Cargo, "IDECARGO");
            Map(x => x.IdeUbigeo, "IDEUBIGEO");
            Map(x => x.PuntajeUbigeo, "PUNTUBIGEO");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Table("UBIGEO_CARGO");
        }
    }
}