
namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class ReclutamientoPersonaAlternativaMap : ClassMap<ReclutamientoPersonaAlternativa>
    {
        public ReclutamientoPersonaAlternativaMap()
        {

            Id(m => m.IdeReclutamientoPersonaAlternativa, "IDERECLUPERSOALTERNATIVA")
                    .GeneratedBy
                    .Sequence("IDERECLUPERSOALTERNATIVA_SQ");
            Map(x => x.IdeReclutaPersonaCriterio, "IDERECLUPERSOCRITERIO");
            Map(x => x.IdeAlternativa, "IDEALTERNATIVA");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Table("RECLU_PERSO_ALTERNATIVA");

        }
    }
}
