

namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class UsuarioRolSedeMap : ClassMap<UsuarioRolSede> 
    {

        public UsuarioRolSedeMap()
        {

            Id(m => m.IdUsuarolSede, "IDUSUAROLSEDE")
                    .GeneratedBy
                    .Sequence("USUAROLSEDE_SQ");
            Map(x => x.IdSede, "IDSEDE");
            Map(x => x.IdUsuario, "IDUSUARIO");
            Map(x => x.IdRol, "IDROL");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaModificacion, "FECMODIFICACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICACION");

            Table("USUAROLSEDE");
        

        }
    
    }

    
}
