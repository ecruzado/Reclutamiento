namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class RolMap : ClassMap<Rol>
    {
        public RolMap()
        {
            Id(m => m.IdRol, "IDROL")
                 .GeneratedBy
                 .Sequence("ROL_SQ");
            Map(x => x.CodRol, "CODROL");
            Map(x => x.DscRol, "DSCROL");
            Map(x => x.FlgSede, "FLGSEDE");
            Map(x => x.FlgEstado, "FLGESTADO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICACION");
            Map(x => x.FechaModificacion, "FECMODIFICACION");

            Table("ROL");

        }
    }
}
