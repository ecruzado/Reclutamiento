namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Id(m => m.CodigoUsuario, "IDEUSUARIO");
            References(x => x.Rol).Column("IDEROL");
            References(x => x.Empleado).Column("IDEEMPLEADO ");
            Map(x => x.NombreUsuario, "NOMBRE");
            Map(x => x.ClaveUsuario, "CLAVE");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            Table("USUARIO");
        }
    }
}