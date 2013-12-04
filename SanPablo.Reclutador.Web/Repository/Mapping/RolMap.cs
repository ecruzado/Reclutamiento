namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class RolMap : ClassMap<Rol>
    {
        public RolMap()
        {
            Id(m => m.CodigoRol, "IDEROL");
            Map(x => x.NombreRol, "NOMBRE");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            HasManyToMany(x => x.Opciones)
                .Cascade.All()
                .Inverse()
                .Table("ROL_OPCION");
            Table("ROL");
        }
    }
}
