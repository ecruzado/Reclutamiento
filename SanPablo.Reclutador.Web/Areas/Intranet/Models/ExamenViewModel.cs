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

    public class ExamenViewModel
    {
        public Categoria Categoria { get; set; }
        public SubCategoria SubCategoria { get; set; }
        public Criterio Criterio { get; set; }
        public Examen Examen { get; set; }
        public List<DetalleGeneral> TipoEstado { get; set; }
        public List<DetalleGeneral> TipoExamen { get; set; }


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
        
        /// <summary>
        /// obtiene el pdf del examen
        /// </summary>
        public string btnGetExamen { get; set; }

    }
}