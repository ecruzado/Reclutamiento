using FluentValidation;
using FluentValidation.Results;
using NHibernate.Criterion;
using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Entity.Validation;
using SanPablo.Reclutador.Repository.Interface;


namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class CategoriaViewModel
    {
        public Criterio Criterio { get; set; }
        public Categoria Categoria { get; set; }
        public Examen Examen { get; set; }
        public SubCategoria SubCategoria { get; set; }
        public string IndVisual { get; set; }
        
        public Alternativa Alternativa { get; set; }
        public List<DetalleGeneral> TipoCategoria { get; set; }
        public List<DetalleGeneral> TipoEjemplo { get; set; }
        public List<DetalleGeneral> TipoCriterio { get; set; }
        public List<DetalleGeneral> TipoEstado { get; set; }

        public string NombreTempImgCategoria { get; set; }

        public HttpPostedFileBase image { get; set; }


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