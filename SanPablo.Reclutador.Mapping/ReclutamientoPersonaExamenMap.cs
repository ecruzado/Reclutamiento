

namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;
    
    public class ReclutamientoPersonaExamenMap : ClassMap<ReclutamientoPersonaExamen>
    {
        public ReclutamientoPersonaExamenMap()
        {

            Id(m => m.IdeReclutamientoPersonaExamen, "IDERECLUPERSOEXAMEN")
                    .GeneratedBy
                    .Sequence("IDERECLUPERSOEXAMEN_SQ");
            Map(x => x.IdeReclutamientoPersona, "IDERECLUTAPERSONA");
            Map(x => x.IdeEvaluacion, "IDEEVALUACION");
            Map(x => x.TipoSolicitud, "TIPSOLICITUD");
            Map(x => x.IdeUsuarioResponsable, "IDUSUARESPONS");
            Map(x => x.FechaEvaluacion, "FECEVALUACION");
            Map(x => x.HoraEvaluacion, "HORAEVALUACION");
            Map(x => x.Observacion, "OBSERVACION");
            Map(x => x.NotaFinal, "NOTAFINAL");
            Map(x => x.Archivo, "ARCHIVO");
            Map(x => x.nombreArchivo, "NOMARCHIVO");
            Map(x => x.ComentarioResultado, "COMENTARIORESUL");
            Map(x => x.TipoEstadoEvaluacion, "TIPESTEVALUACION");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");
          
            Table("RECLU_PERSO_EXAMEN");
          
        }
    }
}
