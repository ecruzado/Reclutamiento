﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;


namespace SanPablo.Reclutador.Entity
{
    
    public class ReportePostulantePotencial:BaseEntity
    {
        public virtual int IdeReclutaPersona { get; set; }

        public virtual int IdePostulante { get; set; }

        public virtual string FechaPostulantePotencial { get; set; }
        
        public virtual DateTime? FechaDesde { get; set; }
        
        public virtual DateTime? FechaHasta { get; set; }

        public virtual string Sede { get; set; }
        
        public virtual string Dependencia { get; set; }
        
        public virtual string Departamento { get; set; }

        public virtual string Area { get; set; }

        public virtual int? IdeSede { get; set; }

        public virtual int? IdeDependencia{ get; set; }

        public virtual int? IdeDepartamento { get; set; }

        public virtual int? IdeArea { get; set; }

        public virtual string NombreCompleto { get; set; }

        public virtual int? IdeCargo { get; set; }

        public virtual string Cargo { get; set; }
        
        public virtual string TelefonoContacto { get; set; }

        public virtual string Email { get; set; }
        
        public virtual int? Edad { get; set; }

        public virtual int? EdadInicio { get; set; }

        public virtual int? EdadFin { get; set; }

        public virtual int? PuntajeCV { get; set; }

        public virtual int? PuntajeSeleccion { get; set; }

        public virtual string TipoArea { get; set; }
        
        public virtual string AreaEstudio { get; set; }

        public virtual string TipoRangoSalario { get; set; }
        
        public virtual string RangoSalarial { get; set; }

        public virtual string TipoEstudio { get; set; }

    }
}