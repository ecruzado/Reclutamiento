

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    

    public class CriterioViewModel
    {
        public Criterio Criterio { get; set; }
        public virtual string IndVisual { get; set; }
        public Alternativa Alternativa { get; set; }
        public CriterioPorSubcategoria CriterioPorSubcategoria { get; set; }
        public virtual List<DetalleGeneral> TipoCriterio { get; set; }
        public virtual List<DetalleGeneral> Medicion { get; set; }
        public virtual List<DetalleGeneral> Estado { get; set; }
        public virtual List<DetalleGeneral> TipoModo { get; set; }
        public virtual List<DetalleGeneral> TipoCalificacion { get; set; }
        public HttpPostedFileBase image { get; set; }
        public string NombreTemporalArchivo { get; set; }
        public HttpPostedFileBase ImagenAlternativa { get; set; }
        public virtual string imagen2 { get; set; }
        public virtual string MensajeVal{ get; set; }


        //botones

        /// <summary>
        /// boton Buscar
        /// </summary>
        public string btnBuscar { get; set; }
        /// <summary>
        /// boton limpiar
        /// </summary>
        public string btnLimpiar { get; set; }

        /// <summary>
        /// boton nuevo
        /// </summary>
        public string btnNuevo { get; set; }
        /// <summary>
        /// boton Consultar
        /// </summary>
        public string btnConsultar { get; set; }

        /// <summary>
        /// boton editar
        /// </summary>
        public string btnEditar { get; set; }
        /// <summary>
        /// boton Eliminar
        /// </summary>
        public string btnEliminar { get; set; }

        /// <summary>
        /// boton Activar Desactivar
        /// </summary>
        public string btnActivarDesactivar { get; set; }
        
        
    }
}