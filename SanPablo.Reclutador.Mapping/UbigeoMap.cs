namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class UbigeoMap : ClassMap<Ubigeo>
    {
        public UbigeoMap()
        {
            Id(m => m.IdeUbigeo, "IDEUBIGEO");
            Map(x => x.IdeUbigeoPadre,"IDEUBIGEOPADRE");
            Map(x => x.Nombre, "NOMBRE");
            Map(x => x.Codigo, "CODIGO");
            Table("UBIGEO");
        }
    }
}