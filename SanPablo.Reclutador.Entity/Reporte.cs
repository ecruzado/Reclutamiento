
namespace SanPablo.Reclutador.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    
    public class Reporte : BaseEntity
    {
        /// <summary>
        /// FechaInicio

        public virtual string FechaInicio { get; set; }
        /// <summary>
        /// numero de meses
        /// </summary>
        public virtual int NumberMonth { get; set; }

        /// <summary>
        /// FechaFin

        public virtual string FechaFin { get; set; }

        /// <summary>
        /// id de la sede
        /// </summary>
        public virtual int  idSede { get; set; }
        /// <summary>
        /// id del tipo de solicitud
        /// </summary>
        public virtual string  idTipSol { get; set; }

        /// <summary>
        /// id Estado del requerimiento
        /// </summary>
        public virtual string idEstadoReq { get; set; }

        /// <summary>
        /// id del analista responsable
        /// </summary>
        public virtual int idAnalistaResp { get; set; }

        /// <summary>
        /// id de la dependencia
        /// </summary>
        public virtual int idDependencia { get; set; }
        /// <summary>
        /// id del departamento
        /// </summary>
        public virtual int idDepartamento { get; set; }
        /// <summary>
        /// id del area
        /// </summary>
        public virtual int idArea { get; set; }
        /// <summary>
        /// id del motivo de reeemplazo
        /// </summary>
        public virtual string idMotivoReemplazo { get; set; }
        /// <summary>
        /// id de la solicitud
        /// </summary>
        public virtual string IdeSolReqpersonal { get; set; }
        /// <summary>
        /// estado del proceso
        /// </summary>
        public virtual string EstadoProceso { get; set; }
        /// <summary>
        /// fecha de creacion de la solicitud
        /// </summary>
        public virtual string FechaRequerimiento { get; set; }
        /// <summary>
        /// descripcion de la sede para el reporte
        /// </summary>
        public virtual string DesSede { get; set; }
        /// <summary>
        /// Descripcion de la dependencia
        /// </summary>
        public virtual string DesDependencia { get; set; }

        /// <summary>
        /// descripcion del departamento
        /// </summary>
        public virtual string DesDepartamento { get; set; }

        /// <summary>
        /// descripcion del area
        /// </summary>
        public virtual string DesArea { get; set; }

        /// <summary>
        /// Descripcion del cargo
        /// </summary>
        public virtual string Cargo { get; set; }
        /// <summary>
        /// nombre del jefe que genero la solicitud
        /// </summary>
        public virtual string Jefe { get; set; }
        /// <summary>
        /// el tipo de solicitud
        /// </summary>
        public virtual string Tipsol { get; set; }

        /// <summary>
        /// persona a la que reemplaza en caso sea un tipo de reemplazo
        /// </summary>
        public virtual string Reemplaza { get; set; }

        /// <summary>
        /// fecha de reemplazo
        /// </summary>
        public virtual string FecReemplazo { get; set; }

        /// <summary>
        /// motivo de reemplazo
        /// </summary>
        public virtual string MotivoReemplazo { get; set; }

        /// <summary>
        /// analista responsable de la solictud
        /// </summary>
        public virtual string AnalistaResp { get; set; }
        /// <summary>
        /// Nombre de la persona contratada
        /// </summary>
        public virtual string PersonaIngresa { get; set; }
        /// <summary>
        /// fecha de contratacion
        /// </summary>
        public virtual string FechaContratacion { get; set; }
        /// <summary>
        /// numero de dias trasncurridos
        /// </summary>
        public virtual int Dias { get; set; }
        /// <summary>
        /// numero de documento
        /// </summary>
        public virtual string Numdocumento { get; set; }

        /// <summary>
        /// telefono
        /// </summary>
        public virtual string Fono { get; set; }

        /// <summary>
        /// observaciones del psicologo
        /// </summary>
        public virtual string ObsPsicologo { get; set; }

        /// <summary>
        /// observaciones de la entrevista
        /// </summary>
        public virtual string ObsEntrevista { get; set; }

        /// <summary>
        /// la fecha de la ultima modificacion de la solicitud
        /// </summary>
        public virtual string FecSuceso { get; set; }


        /// <summary>
        /// Motivo de cierre de una solicitud
        /// </summary>
        public virtual string MotivoCirreSol { get; set; }


       // parametros para el reporte de resumen de RQ
        /// <summary>
        /// profesional a cargo
        /// </summary>
        public virtual int Profesional { get; set; }

        /// <summary>
        /// saldo a la fecha solicitudes publicadas
        /// </summary>
        public virtual int Saldo { get; set; }
        /// <summary>
        /// cantidad de vacantes de solicitudes que fueron publicadas
        /// </summary>
        public virtual int CantVacPubNuevo { get; set; }
        /// <summary>
        /// cantidad de vacantes para reemplazo
        /// </summary>
        public virtual int CantVacPubReemplazo { get; set; }
        /// <summary>
        /// cantidad de vacantes para Apliacion
        /// </summary>
        public virtual int CantVacPubAmpliacion { get; set; }
        /// <summary>
        /// vacantes de solicitudes finalizadas no cubiertas
        /// </summary>
        public virtual int CantVacFinalNo { get; set; }
        /// <summary>
        /// vacantes de solicitudes finalizadas cubiertas
        /// </summary>
        public virtual int CantVacFinalSi { get; set; }
        /// <summary>
        /// total de vacantes
        /// </summary>
        public virtual int Total { get; set; }

        
       
        

    }
}
