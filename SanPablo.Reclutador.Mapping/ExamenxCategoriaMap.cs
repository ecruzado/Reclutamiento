

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
            Map(x => x.EstActivo,       "ESTACTIVO");
            Map(x => x.UsrCreacion,     "USRCREACION");
            Map(x => x.FecCreacion,     "FECCREACION");
            Map(x => x.UsrModifica,     "USRMODIFICA");
            Map(x => x.FecModifica,     "FECMODIFICA");
            Table("EXAMEN_X_CATEGORIA");

        }
    }
}
