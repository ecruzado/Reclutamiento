

namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class ExamenxCategoriaMap : ClassMap<ExamenPorCategoria>
    {
        public ExamenxCategoriaMap()
        {

            Id(m => m.IdeExamenxCategoria, "IDEEXAMENXCATEGORIA")
                .GeneratedBy
                .Sequence("IDEEXAMENXCATEGORIA_SQ");
            References(m => m.Categoria, "IDECATEGORIA");
            References(m => m.Examen,   "IDEEXAMEN");
            Map(m => m.EstActivo, "ESTACTIVO");
            Map(m => m.UsrCreacion, "USRCREACION");
            Map(m => m.FecCreacion, "FECCREACION");
            Map(m => m.UsrModifica, "USRMODIFICA");
            Map(m => m.FecModifica, "FECMODIFICA");
            Table("EXAMEN_X_CATEGORIA");

        }
    }
}
