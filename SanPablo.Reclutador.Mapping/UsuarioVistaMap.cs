using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanPablo.Reclutador.Mapping
{

    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class UsuarioVistaMap : ClassMap<UsuarioVista> 
    {
        public UsuarioVistaMap()
        {
           
            Id(x => x.IDUSUARIO, "IDUSUARIO");
            Map(x => x.DSCAPEPATERNO, "DSCAPEPATERNO");
            Map(x => x.DSCAPEMATERNO, "DSCAPEMATERNO");
            Map(x => x.DSCNOMBRES, "DSCNOMBRES");
            
            Map(x => x.EMAIL, "EMAIL");
            Map(x => x.CODUSUARIO, "CODUSUARIO");
            Map(x => x.FLGESTADO, "FLGESTADO");
            Map(x => x.IDROL, "IDROL");
            Map(x => x.IDESEDE, "IDESEDE");
            Map(x => x.DESROL, "DESROL");
            Map(x => x.DESSEDE, "DESSEDE");

            Table("VISTA_USUARIO");
        }


    }
}
