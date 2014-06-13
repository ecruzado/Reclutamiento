namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class EvaluacionCargoMap : ClassMap<EvaluacionCargo>
    {
        public EvaluacionCargoMap()
        {

            Id(m => m.IdeEvaluacionCargo, "IDEEVALUACIONCARGO")
                .GeneratedBy
                .Sequence("IDEEVALUACIONCARGO_SQ");
            References(x => x.Cargo, "IDECARGO");
            References(x => x.Examen, "IDEEXAMEVAL");
            Map(x => x.TipoExamen, "TIPEXAMEN");
            Map(x => x.TipoAreaResponsable, "TIPAREARESPON");
            Map(x => x.NotaMinimaExamen, "NOTAMINEXAMEN");
            Map(x => x.PuntajeExamen, "PUNTEXAMEN");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Map(x => x.IndEntrevFinal, "INDENTREVFINAL");
            

            Map(x => x.DescripcionExamen).Formula("(SELECT E.DESCEXAMEN FROM EXAMEN E WHERE E.IDEEXAMEN = IDEEXAMEVAL )");
            Map(x => x.DescripcionTipoExamen).Formula("(select chsprp.pr_intranet.sp_lista_lval(" + (int)TipoTabla.TipoCriterio + ",TIPEXAMEN) from dual)");
                     
            Table("EVALUACION_CARGO");

        }
    }
}