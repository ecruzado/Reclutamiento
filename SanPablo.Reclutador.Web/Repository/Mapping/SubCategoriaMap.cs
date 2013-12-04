namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class SubCategoriaMap : ClassMap<SubCategoria>
    {
        public SubCategoriaMap()
        {
            Id(m => m.CodigoSubCategoria, "IDESUBCATEGORIA");
            References(x => x.Categoria).Column("IDECATEGORIA");
            Map(x => x.NombreSubCategoria, "NOMBRE");
            Map(x => x.DescripcionSubCategoria, "DESCRIPCION");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            HasManyToMany(x => x.Criterios)
                .Cascade.All()
                .Inverse()
                .Table("CRITERIO_X_SUBCATEGORIA");
            Table("SUBCATEGORIA");
        }
    }
}