namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class DetalleGeneralMap : ClassMap<DetalleGeneral>
    {
        public DetalleGeneralMap()
        {
            Id(m => m.CodigoDetalleGeneral, "CODDETALLEGENERAL");
            References(x => x.General).Column("CODEGENERAL");
            References(x => x.DetallePadre).Column("CODDETALLEPADRE");
            Map(x => x.NombreDetalleGeneral, "NOMBRE");
            Map(x => x.LongitudCampo, "LONCAMPO");
            Map(x => x.TipoCampo, "TIPDATO");
            Map(x => x.Abreviatura, "ABREVIATURA");
            Map(x => x.Simbolo, "SIMBOLO");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            Table("DETALLE_GENERAL");
        }

    }
}
