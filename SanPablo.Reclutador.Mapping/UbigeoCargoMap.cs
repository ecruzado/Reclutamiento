namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class UbigeoCargoMap : ClassMap<UbigeoCargo>
    {
        public UbigeoCargoMap()
        {
            Id(m => m.IdeUbigeoCargo, "IDEUBIGEOCARGO")
                .GeneratedBy
                .Sequence("IDEUBIGEOCARGO_SQ");
            References(x => x.Cargo, "IDECARGO");
            Map(x => x.IdeUbigeo, "IDEUBIGEO");
            Map(x => x.PuntajeUbigeo, "PUNTUBIGEO");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Map(x => x.Departamento).Formula("");
            Map(x => x.Provincia).Formula("");
            Map(x => x.Distrito).Formula("");

            Table("UBIGEO_CARGO");
        }
    }
}