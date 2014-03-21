

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
            Map(x => x.IdeEvaluacionSolicitudRequerimiento, "IDEEVALUACIONSOLREQ");
            Map(x => x.IdeEvaluacionCargo, "IDEEVALUACIONCARGO");
            Map(x => x.IdeRolResponsable, "IDROLRESPONSABLE");
            Map(x => x.FechaEvaluacion, "FECEVALUACION");
            Map(x => x.HoraEvaluacion, "HORAEVALUACION");
            Map(x => x.NotaFinal, "NOTAFINAL");
            Map(x => x.Archivo, "ARCHIVO");
            Map(x => x.ComentarioResultado, "COMENTARIORESUL");
            Map(x => x.EstadoEvaluacion, "ESTADOEVALUACION");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");
          
            Table("RECLU_PERSO_EXAMEN");
          
        }
    }
}
