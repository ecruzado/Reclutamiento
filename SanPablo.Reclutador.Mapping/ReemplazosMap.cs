

namespace SanPablo.Reclutador.Mapping
{

    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    
    public class ReemplazosMap : ClassMap<Reemplazo>
    {
        public ReemplazosMap()
        {
            Id(m => m.IdReemplazo, "IDREEMPLAZO")
                .GeneratedBy
                .Sequence("REEMPLAZO_SQ");
            Map(x => x.IdeSolReqPersonal, "IDESOLREQPERSONAL");
            
            Map(x => x.ApePaterno, "APEPATERNO");
            Map(x => x.ApeMaterno, "APEMATERNO");
            Map(x => x.Nombres, "NOMBRES");
            Map(x => x.FecInicioReemplazo, "FECINICIOREEMPLAZO");
            Map(x => x.FecFinalReemplazo, "FECFINREEMPLAZO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICACION");
            Map(x => x.FechaModificacion, "FECMODIFICACION");
            Table("REEMPLAZOS");

        }
    }

}
