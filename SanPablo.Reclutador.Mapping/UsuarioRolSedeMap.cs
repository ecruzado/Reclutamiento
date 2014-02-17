

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
            Map(x => x.IdSede, "IDESEDE");
            Map(x => x.IdUsuario, "IDUSUARIO");
            Map(x => x.IdRol, "IDROL");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaModificacion, "FECMODIFICACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICACION");
            Map(x => x.RolDes).Formula("(select r.codRol from rol r where r.idRol = IDROL)");
            Map(x => x.SedeDes).Formula("(select s.descripcion from sede s where s.idesede = IDESEDE)");
                



            Table("USUAROLSEDE");
        

        }
    
    }

    
}
