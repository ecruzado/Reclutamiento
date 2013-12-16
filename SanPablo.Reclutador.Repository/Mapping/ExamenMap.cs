namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class ExamenMap : ClassMap<Examen>
    {
        public ExamenMap()
        {
            Id(m => m.CodigoExamen, "IDECRITERIO");
            References(x => x.Sede).Column("IDESEDE");
            Map(x => x.NombreExamen, "NOMBRE");
            Map(x => x.DescripcionExamen, "DESCRIPCION");
            Map(x => x.TipoExamen, "TIPEXAMEN");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            HasManyToMany(x => x.Categorias)
                .Cascade.All()
                .Inverse()
                .Table("EXAMEN_X_CATEGORIA");
            Table("EXAMEN");
        }
    }
}