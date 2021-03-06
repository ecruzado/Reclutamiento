﻿

namespace SanPablo.Reclutador.Mapping
{

    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class RolOpcionMap : ClassMap<RolOpcion>
    {
        public RolOpcionMap()
        {
            Id(m => m.IDROLOPCION, "IDROLOPCION")
                .GeneratedBy
                .Sequence("ROLOPCIONES_SQ");
            Map(x => x.IDROL, "IDROL");
            Map(x => x.IDOPCION, "IDOPCION");
            Map(x => x.NombreOpcion).Formula("(select op.dscopcion from opciones op where op.idopcion =IDOPCION)");
            Map(x => x.DescOpcion).Formula("(select op.descripcion from opciones op where op.idopcion =IDOPCION)");
            Map(x => x.USRCREACION, "USRCREACION");
            Map(x => x.FECCREACION, "FECCREACION");
            Map(x => x.USRMODIFICACION, "USRMODIFICACION");
            Map(x => x.FECMODIFICACION, "FECMODIFICACION");
            Table("ROLOPCION");

        }
    }
}
