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
            Map(x => x.EstadoActivo, "ESTADO");
            Map(x => x.CodigoCargo, "CODCARGO");
            Map(x => x.NombreCargo, "NOMBRE");
            Map(x => x.IdeDependencia, "IDEDEPENDENCIA");
            Map(x => x.NombreDependencia, "NOMDEPENDENCIA");
            Map(x => x.IdeDepartamento, "IDEDEPARTAMENTO");
            Map(x => x.NombreDepartamento, "NOMDEPARTAMENTO");
            Map(x => x.IdeArea, "IDEAREA");
            Map(x => x.NombreArea, "NOMAREA");
            Map(x => x.NumeroPosiciones, "NUMPOSICIONES");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.Responsable, "DSCROL");
            Map(x => x.IdeResponsable, "IDROL");

            Map(x => x.NombreResponsable, "NOMRESPONSABLE");
            Map(x => x.TipoEtapa, "TIPETAPA");
            Map(x => x.Etapa, "TETAPA");
            Map(x => x.TipoEstado, "TIPSUCESO");
            Map(x => x.Estado, "SUCESO");

            Table("LISTA_SOLICITUD_NUEVO_CARGO");
        }


    }
}
