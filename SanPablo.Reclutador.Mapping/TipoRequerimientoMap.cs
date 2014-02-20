﻿

namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    
    public class TipoRequerimientoMap : ClassMap<TipoRequerimiento>
    {

        public TipoRequerimientoMap()
        {
            Id(x => x.IDUSUREQ, "IDUSUREQ")
                .GeneratedBy
                .Sequence("USUARIOREQ_SQ");
            Map(x => x.IDUSUARIO, "IDUSUARIO");
            Map(x => x.TIPREQ, "TIPREQ");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICACION");
            Map(x => x.FechaModificacion, "FECMODIFICACION");

            Table("USUARIOREQ");
            
        }
    }
}
