
namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;
    
    public class ReclutamientoPersonaCriterioMap : ClassMap<ReclutamientoPersonaCriterio>
    {
        public ReclutamientoPersonaCriterioMap()
        {

            Id(m => m.IdeReclutamientoPersonaCriterio, "IDERECLUPERSOCRITERIO")
                    .GeneratedBy
                    .Sequence("IDERECLUPERSOCRITERIO_SQ");
            Map(x => x.IdeReclutaPersona, "IDERECLUTAPERSONA");
            Map(x => x.IdeCriterioXSubcategoria, "IDECRITERIOXSUBCATEGORIA");
            Map(x => x.IdeReclutamientoExamenCategoria, "IDERECLPERSOEXAMNCAT");
            Map(x => x.IndicadorRespuesta, "INDRESPUESTA");
            Map(x => x.PuntajeTotal, "PUNTTOTAL");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Table("RECLU_PERSO_CRITERIO");

        }
    }
}
