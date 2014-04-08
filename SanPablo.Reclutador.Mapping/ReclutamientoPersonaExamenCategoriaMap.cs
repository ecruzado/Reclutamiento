
namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class ReclutamientoPersonaExamenCategoriaMap : ClassMap<ReclutamientoPersonaExamenCategoria>
    {
        public ReclutamientoPersonaExamenCategoriaMap()
        {

            Id(m => m.IdeReclutamientoPersonaExamenCategoria, "IDERECLPERSOEXAMNCAT")
                    .GeneratedBy
                    .Sequence("IDERECLPERSOEXAMNCAT_SQ");
            Map(x => x.IdeReclutaPersona, "IDERECLUTAPERSONA");
            Map(x => x.IdeReclutaPersonaExamen, "IDERECLUPERSOEXAMEN");
            Map(x => x.IdeExamenPorCategoria, "IDEEXAMENXCATEGORIA");
            Map(x => x.Estado, "ESTADO");
            Map(x => x.NumeroPreguntas, "NROPREGUNTAS");
            Map(x => x.Nota, "NOTAEXAMENCATEG");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Table("RECL_PERS_EXAM_CAT");

        }
    }
}
