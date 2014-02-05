namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class CriterioMap : ClassMap<Criterio>
    {
        public CriterioMap()
        {
            Id(m => m.IdeCriterio, "IDECRITERIO")
                .GeneratedBy
                .Sequence("IDECRITERIO_SQ");
            Map(x => x.TipoMedicion, "TIPMEDICION");
            Map(x => x.TipoCriterio, "TIPCRITERIO");
            Map(x => x.TipoModo, "TIPMODO");
            Map(x => x.Pregunta, "PREGUNTA");
            Map(x => x.TipoCalificacion, "TIPCALIFICACION");
            Map(x => x.OrdenImpresion, "ORDENIMPRESION");
            Map(x => x.IndicadorActivo, "ESTACTIVO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICACION");
            Map(x => x.FechaModificacion, "FECMODIFICACION");
            Map(x => x.IMAGENCRIT, "IMAGENCRIT");
            Map(x => x.rutaImagen, "NOMIMAGEN");
            Map(x => x.TipoMedicionDes).Formula("(select chsprp.pr_intranet.sp_lista_lval(" + (int)TipoTabla.Medicion + ",TIPMEDICION) from dual)");
            Map(x => x.TipoCriterioDes).Formula("(select chsprp.pr_intranet.sp_lista_lval(" + (int)TipoTabla.TipoCriterio + ",TIPCRITERIO) from dual)");
            Map(x => x.TipoModoDes).Formula("(select chsprp.pr_intranet.sp_lista_lval(" + (int)TipoTabla.Modo + ",TIPMODO) from dual)");
            Map(x => x.TipoCalificacionDes).Formula("(select chsprp.pr_intranet.sp_lista_lval(" + (int)TipoTabla.TipoCalificacion + ",TIPCALIFICACION) from dual)");
            Table("CRITERIO");
            
        }
    }
}