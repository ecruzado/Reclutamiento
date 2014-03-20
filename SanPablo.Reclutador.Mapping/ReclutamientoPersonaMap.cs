

namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;
    
    public class ReclutamientoPersonaMap : ClassMap<ReclutamientoPersona>
    {
        public ReclutamientoPersonaMap()
        {

            Id(m => m.IdeReclutaPersona, "IDERECLUTAPERSONA")
                    .GeneratedBy
                    .Sequence("IDERECLUTAPERSONA_SQ");
            Map(x => x.IdePostulante, "IDEPOSTULANTE");
            Map(x => x.IdeSol, "IDESOL");
            Map(x => x.TipSol, "TIPSOL");
            Map(x => x.IdeCargo, "IDECARGO");
            Map(x => x.IdeCv, "IDECV");
            Map(x => x.EstActivo, "ESTACTIVO");
            Map(x => x.EstPostulante, "ESTPOSTULANTE");
            Map(x => x.IndContactado, "INDCONTACTADO");
            Map(x => x.Evaluacion, "EVALUACION");
            Map(x => x.PtoTotal, "PTOTOTAL");
            Map(x => x.Comentario, "COMENTARIO");
            Map(x => x.TipPuesto, "TIPPUESTO");
            Map(x => x.IdSede, "IDSEDE");
            Map(x => x.UsrModifica, "USRMODIFICA");
            Map(x => x.FecModifica, "FECMODIFICA");
            Map(x => x.FecCreacion, "FECCREACION");
          
            Table("RECLUTAMIENTO_PERSONA");
          
           
        }
    }
}
