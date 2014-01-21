namespace SanPablo.Reclutador.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class GeneralMap : ClassMap<General>
    {
        public GeneralMap()
        {
            Id(m => m.IdeGeneral, "IDEGENERAL");
            Map(x => x.TipoTabla, "TIPTABLA");
            Map(x => x.Descripcion, "DESCRIPCION");
            Map(x => x.TipoDato, "TIPDATO");
            Map(x => x.LongitudCampo, "LONCAMPO");
            Table("GENERAL");
        }
    }
}
