namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class CriterioPorSubcategoriaMap : ClassMap<CriterioPorSubcategoria>
    {
        public CriterioPorSubcategoriaMap()
        {
            Id(m => m.IDECRITERIOXSUBCATEGORIA, "IDECRITERIOXSUBCATEGORIA")
                .GeneratedBy
                .Sequence("CRITERIOXSUBCATEGORIA_SQ");
            References(x => x.Criterio, "IDECRITERIO");
            References(x => x.SubCategoria, "IDESUBCATEGORIA");
            Map(x => x.PUNTAMAXIMO, "PUNTAMAXIMO");
            Map(x => x.PRIORIDAD, "PRIORIDAD");
            Map(x => x.ESTREGISTRO, "ESTREGISTRO");
            Map(x => x.USRCREACION, "USRCREACION");
            Map(x => x.FECCREACION, "FECCREACION");
            Map(x => x.USRMODIFICA, "USRMODIFICA");
            Map(x => x.FECMODIFICA, "FECMODIFICA");
            Map(x => x.PUNTAJECAL).Formula("(SELECT CHSPRP.PR_INTRANET.FN_GETMAXPUNTAJE(IDECRITERIO) FROM DUAL)");
           
            Table("CRITERIO_X_SUBCATEGORIA");

        }
    }
}