namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class OpcionMap : ClassMap<Opcion>
    {
        public OpcionMap()
        {
            Id(m => m.CodigoOpcion, "IDEOPCION");
            References(x => x.OpcionPadre).Column("IDEOPCIONPADRE");
            Map(x => x.NombreOpcion, "NOMBRE");
            Map(x => x.TipoOpcion, "TIPOPCION");
            Map(x => x.Titulo, "TITULO");
            Map(x => x.Ruta, "RUTA");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            HasManyToMany(x => x.Roles)
                .Cascade.All()
                .Inverse()
                .Table("ROL_OPCION");
            Table("OPCION");
        }
    }
}
