using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanPablo.Reclutador.Mapping
{

    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class ListaSolicitudNuevoCargoVistaMap : ClassMap<ListaSolicitudNuevoCargo> 
    {
        public ListaSolicitudNuevoCargoVistaMap()
        {

            Id(x => x.IdeSolicitudNuevoCargo, "IDESOLNUEVOCARGO");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Map(x => x.CodigoCargo, "CODCARGO");
            Map(x => x.NombreCargo, "NOMBRE");

            Map(x => x.NombreDependencia, "NOMDEPENDENCIA");
            Map(x => x.NombreDepartamento, "NOMDEPARTAMENTO");
            Map(x => x.NombreArea, "NOMAREA");
            Map(x => x.NumeroPosiciones, "NUMPOSICIONES");
            Map(x => x.FechaCreacion, "FECCREACION");

            Table("LISTA_SOLICITUD_NUEVO_CARGO");
        }


    }
}
