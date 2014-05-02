

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
 
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    
    public class SolicitudRempCargoViewModel
    {
        public SolReqPersonal SolReqPersonal { get; set; }
        public Reemplazo Reemplazo { get; set; }
        public LogSolReqPersonal LogSolReqPersonal { get; set; }

        public List<DetalleGeneral> listaTipPuesto { get; set; }
        public List<DetalleGeneral> listaTipVacante { get; set; }
        public List<Dependencia> Dependencias { get; set; }
        public List<Departamento> Departamentos { get; set; }
        public List<Area> Areas { get; set; }
        public List<Cargo> listaTipCargo { get; set; }

        public List<Dependencia> listaDependencia { get; set; }
        public List<Departamento> listaDepartamento { get; set; }
        public List<Area> listaArea { get; set; }
        public List<Rol> listaRol { get; set; }
        public List<DetalleGeneral> listaEtapas { get; set; }
        public List<DetalleGeneral> listaEstados { get; set; }
        public List<DetalleGeneral> Sexos { get; set; }
        public List<DetalleGeneral> listaRangoSalarial { get; set; }
        public List<DetalleGeneral> TiposRequerimientos { get; set; }
      
        public string Accion { get; set; }
        public string AccionPopup { get; set; }

        public string Visualiza { get; set; }

        public Boolean verSalario { get; set; }

        public List<DetalleGeneral> RangoSalariales { get; set; }

        public List<DetalleGeneral> Etapas { get; set; }
        public List<DetalleGeneral> Estados { get; set; }

        public string Pagina { get; set; }
        public string TipoReemplazo { get; set; }
       
        #region Botonera de busqueda
        /// <summary>
        /// boton ranking
        /// </summary>
        public string btnRanking { get; set; }
        /// <summary>
        /// boton Preseleccion
        /// </summary>
        public string btnPreselec { get; set; }
        /// <summary>
        /// boton Nuevo
        /// </summary>
        public string btnNuevo { get; set; }
        /// <summary>
        /// boton Requerimiento
        /// </summary>
        public string btnRequerimiento { get; set; }
        /// <summary>
        /// boton Activar
        /// </summary>
        public string btnActivar { get; set; }
        /// <summary>
        /// boton Eliminar
        /// </summary>
        public string btnEliminar { get; set; }
        /// <summary>
        /// boton publicar -  Publicación
        /// </summary>
        public string btnPublicar { get; set; }
        /// <summary>
        /// boton Actualizar - Publicación
        /// </summary>
        public string btnActualizar { get; set; }
        #endregion

    }




}