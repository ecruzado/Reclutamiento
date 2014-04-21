

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using FluentValidation;
    using FluentValidation.Results;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;


    public class ReporteViewModel
    {
        /// <summary>
        /// clase solicitud
        /// </summary>
        public SolReqPersonal Solicitud { get; set; }

        /// <summary>
        /// clase reporte
        /// </summary>
        public Reporte ReporteSol { get; set; }


        /// <summary>
        /// lista de sedes
        /// </summary>
        public List<Sede> ListaSede { get; set; }
        /// <summary>
        /// lista de tipo de solicitudes
        /// </summary>
        public List<DetalleGeneral> ListaTipoSol { get; set; }

        /// <summary>
        /// lista de estado de requerimiento
        /// </summary>
        public List<DetalleGeneral> ListaEstadoReq { get; set; }

        /// <summary>
        /// lista de analista responsable
        /// </summary>
        public List<Usuario> ListaAnalistaResp { get; set; }

        /// <summary>
        /// lista de dependencias
        /// </summary>
        public List<Dependencia> listaDependencia { get; set; }

        /// <summary>
        /// lista de departamentos
        /// </summary>
        public List<Departamento> ListaDepartamento { get; set; }

        /// <summary>
        /// lista de areas
        /// </summary>
        public List<Area> ListaArea { get; set; }

        /// <summary>
        /// lista de motivo de reemplazo
        /// </summary>
        public List<DetalleGeneral> ListaMotivo { get; set; }
       

    }
}