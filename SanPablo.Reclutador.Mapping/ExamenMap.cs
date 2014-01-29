namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class ExamenMap : ClassMap<Examen>
    {
        public ExamenMap()
        {
            
            Id(m => m.IdeExamen, "IDEEXAMEN")
                .GeneratedBy
                .Sequence("IDEEXAMEN_SQ");
            Map(x => x.IdeSede, "IDESEDE");
            Map(x => x.EstRegistro, "ESTREGISTRO");
            Map(x => x.NomExamen, "NOMEXAMEN");
            Map(x => x.DescExamen, "DESCEXAMEN");
            Map(x => x.TipExamen, "TIPEXAMEN");
            Map(x => x.EstActivo, "ESTACTIVO");
            Map(x => x.UsrCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsrModificacion, "USRMODIFICACION");
            Map(x => x.FechaModificacion, "FECMODIFICACION");
            Table("EXAMEN");

        }
    }
}