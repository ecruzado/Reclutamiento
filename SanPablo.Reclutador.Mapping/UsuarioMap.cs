namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Id(m => m.IdUsuario, "IDUSUARIO")
                .GeneratedBy
                .Sequence("USUARIO_SQ");
            Map(x => x.CodUsuario, "CODUSUARIO");
            Map(x => x.CodContrasena, "CODCONTRASENA");
            Map(x => x.DscApePaterno, "DSCAPEPATERNO");
            Map(x => x.DscApeMaterno, "DSCAPEMATERNO");
            Map(x => x.DscNombres, "DSCNOMBRES");
            Map(x => x.Email, "EMAIL");
            Map(x => x.Telefono, "TELEFONO");
            Map(x => x.FlgEstado, "FLGESTADO");
            Map(x => x.UsrCreacion, "USRCREACION");
            Map(x => x.FecCreacion, "FECCREACION");
            Map(x => x.UsrModificacion, "USRMODIFICACION");
            Map(x => x.FecModifcacion, "FECMODIFICACION");
            Map(x => x.TipUsuario, "TIPUSUARIO");
            

            Table("USUARIO");
            
        }
    }
}