namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class CategoriaMap : ClassMap<Categoria>
    {
        public CategoriaMap()
        {
            Id(x => x.IDECATEGORIA, "IDECATEGORIA")
              .GeneratedBy
              .Sequence("IDECATEGORIA_SQ");
            Map(x => x.ORDENIMPRESION, "ORDENIMPRESION");
            Map(x => x.NOMCATEGORIA, "NOMCATEGORIA");
            Map(x => x.DESCCATEGORIA, "DESCCATEGORIA");
            Map(x => x.TIPCATEGORIA, "TIPCATEGORIA");
            Map(x => x.ESTACTIVO, "ESTACTIVO");
            Map(x => x.USRCREACION, "USRCREACION");
            Map(x => x.FECCREACION, "FECCREACION");
            Map(x => x.USRMODIFICA, "USRMODIFICA");
            Map(x => x.FECMODIFICA, "FECMODIFICA");
            Map(x => x.INSTRUCCIONES, "INSTRUCCIONES");
            Map(x => x.TIPOEJEMPLO, "TIPOEJEMPLO");
            Map(x => x.IMAGENEJEMPLO, "IMAGENEJEMPLO");
            Map(x => x.TEXTOEJEMPLO, "TEXTOEJEMPLO");
            Map(x => x.TIPCATEGORIADES).Formula("(select chsprp.pr_intranet.sp_lista_lval(" + (int)TipoTabla.TipoCategoria + ",TIPCATEGORIA) from dual)");
            
           /* HasManyToMany(x => x.ExamenesCategoria)
                .Cascade.All()
                .Inverse()
                .Table("EXAMEN_X_CATEGORIA");
            HasMany(x => x.SubCategorias)
                .Inverse()
                .Cascade.All();
            Table("CATEGORIA");
            * */
            Table("CATEGORIA");

        }
    }
}