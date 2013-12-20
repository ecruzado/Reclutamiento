namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class UbigeoMap : ClassMap<Ubigeo>
    {
        public UbigeoMap()
        {
            Id(m => m.CodigoUbigeo, "IDEUBIGEO");
            Map(x => x.NombreUbigeo, "NOMBRE");
            References(x => x.UbigeoPadre).Column("IDEUBIGEOPADRE");
            Map(x => x.Codigo, "CODIGO");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            Table("UBIGEO");
        }
    }
}