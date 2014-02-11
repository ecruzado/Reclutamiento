namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class OpcionMap : ClassMap<Opcion>
    {
        public OpcionMap()
        {
            Id(m => m.IDITEM, "IDITEM")
                .GeneratedBy
                .Assigned();
            Map(x => x.IDOPCION, "IDOPCION");
            Map(x => x.IDOPCIONPADRE, "IDOPCIONPADRE");
            Map(x => x.DSCOPCION, "DSCOPCION");
            Map(x => x.FLGHABILITADO, "FLGHABILITADO");
            Map(x => x.DSCICONO, "DSCICONO");
            Map(x => x.DSCURL, "DSCURL");
            Map(x => x.DESCRIPCION, "DESCRIPCION");
            Map(x => x.USRCREACION, "USRCREACION");
            Map(x => x.FECCREACION, "FECCREACION");
            Map(x => x.USRMODIFICACION, "USRMODIFICACION");
            Map(x => x.FECMODIFICACION, "FECMODIFICACION");

            //HasManyToMany(x => x.Roles)
            //    .Cascade.All()
            //    .Inverse()
            //    .Table("ROL_OPCION");
            Table("OPCIONES");
        }



    }
}
