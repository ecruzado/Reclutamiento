

namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using System.Web;

    public class SeguridadViewModel
    {

        public UsuarioExtranet UsuarioExtranet { get; set; }
        public Rol Rol { get; set; }
        public Sede Sede { get; set; }
        public virtual List<Rol> listaRol { get; set; }
        public virtual List<Sede> listaSede { get; set; }
        public string Accion { get; set; }
        public string Visualicion { get; set; }
        public string IndSede { get; set; }
        public List<MenuItem> listaMenu { get; set; }
        public List<MenuPadre> listaPadre { get; set; }

    }
}