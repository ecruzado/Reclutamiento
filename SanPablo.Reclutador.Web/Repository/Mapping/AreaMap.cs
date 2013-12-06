namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class AreaMap : ClassMap<Area>
    {
        public AreaMap()
        {
            Id(m => m.CodigoArea, "IDEAREA");
            References(x => x.Departamento).Column("IDEDEPARTAMENTO");
            Map(x => x.NombreArea, "NOMBRE");
            Map(x => x.EstadoDeRegistro, "ESTREGISTRO");
            Table("AREA");
        }
    }
}