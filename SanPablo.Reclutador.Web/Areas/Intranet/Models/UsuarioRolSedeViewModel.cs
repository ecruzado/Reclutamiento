

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    

    public class UsuarioRolSedeViewModel
    {
        public UsuarioRolSede UsuarioRolSede { get; set; }
        public Usuario Usuario { get; set; }
        public Password Password { get; set; }
        public TipoRequerimiento TipoReq { get; set; }
        public SedeNivel SedeNivel { get; set; }
        public List<Rol> TipRol { get; set; }
        public List<Sede> TipSede { get; set; }
        public List<Dependencia> Dependencias { get; set; }
        public List<Departamento> Departamentos { get; set; }
        public List<Area> Areas { get; set; }
      
        public int IdUsuario { get; set; }
        public int IdRolUsuario { get; set; }
        public string IndSede { get; set; }


        //accesos de los botones
        /// <summary>
        /// boton que realiza la busqueda
        /// </summary>
        public string btnBuscar { get; set; }
        /// <summary>
        /// boton que realiza la limpieza de los campos
        /// </summary>
        public string btnLimpiar { get; set; }
        /// <summary>
        /// boton que realiza la activacion del usuario
        /// </summary>
        public string btnActivarDesactivar { get; set; }
        /// <summary>
        /// boton que crea un nuevo usuario
        /// </summary>
        public string btnNuevo { get; set; }
        /// <summary>
        /// boton que realiza la edicion
        /// </summary>
        public string btnEditar { get; set; }

        /// <summary>
        /// boton que realiza la consulta
        /// </summary>
        public string btnConsultar { get; set; }

        /// <summary>
        /// boton que realiza la eliminacion
        /// </summary>
        public string btnEliminar { get; set; }
        

    }
}