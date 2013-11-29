namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class CategoriaMap : ClassMap<Categoria>
    {
        public CategoriaMap()
        {
            Id(m => m.CodigoCategoria, "IDECATEGORIA");
            Map(x => x.NombreCategoria, "NOMBRE");
            Map(x => x.DescripcionCategoria, "DESCRIPCION");
            Map(x => x.TipoDeCategoria, "TIPCATEGORIA");
            Map(x => x.EstadoDeRegistro, "ESTREGISTRO");
            Table("CATEGORIA");
        }
    }
}