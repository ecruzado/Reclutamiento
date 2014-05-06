
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class RolViewModel
    {

        public Rol rol { get; set; }
        public virtual List<DetalleGeneral> listaIndSede { get; set; }
        public string Accion { get; set; }

        // accesos de los botones
        public string btnNuevo { get; set; }
        public string btnConsultar { get; set; }
        public string btnEliminar { get; set; }
        public string btnEditar { get; set; }
        



    }
}