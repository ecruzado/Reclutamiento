namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class SubCategoriaMap : ClassMap<SubCategoria>
    {
        public SubCategoriaMap()
        {
            Id(m => m.IDESUBCATEGORIA, "IDESUBCATEGORIA")
                .GeneratedBy
                .Sequence("IDESUBCATEGORIA_SQ");
            References(m => m.Categoria, "IDECATEGORIA");
            Map(x => x.ORDENIMPRESION,   "ORDENIMPRESION");
            Map(x => x.NOMSUBCATEGORIA,  "NOMSUBCATEGORIA");
            Map(x => x.DESCSUBCATEGORIA, "DESCSUBCATEGORIA");
            Map(x => x.ESTACTIVO,        "ESTACTIVO");
            Map(x => x.USRCREACION,      "USRCREACION");
            Map(x => x.FECCREACION,      "FECCREACION");
            Map(x => x.USRMODIFICACION,  "USRMODIFICACION");
            Map(x => x.FECMODIFICACION,  "FECMODIFICACION");
            Table("SUBCATEGORIA");
           
        }
    }
}