﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SanPablo.Reclutador.Entity
{
    
    public class Postulante
    {
        public virtual int IdePostulante { get; set; }
        public virtual string TipoDocumento { get; set; }
        public virtual string NumeroDocumento { get; set; }
        public virtual string ApellidoPaterno { get; set; }
        public virtual string ApellidoMaterno { get; set; }
        public virtual string PrimerNombre { get; set; }
        public virtual string SegundoNombre { get; set; }
        public virtual DateTime FechaNacimiento { get; set; }
        public virtual string NumeroLicencia { get; set; }
        public virtual string IndicadorSexo { get; set; }
        public virtual string TipoEstadoCivil { get; set; }
        public virtual string TipoVia { get; set; }
        public virtual string NombreVia { get; set; }
        public virtual int NumeroDireccion { get; set; }
        public virtual int InteriorDireccion { get; set; }
        public virtual string Manzana { get; set; }
        public virtual string Lote { get; set; }
        public virtual string Bloque { get; set; }
        public virtual string Etapa { get; set; }

        public virtual string Correo { get; set; }
        public virtual string Observacion { get; set; }
        public virtual int TelefonoMovil { get; set; }
        public virtual int TelefonoFijo { get; set; }
        public virtual string TipoZona { get; set; }
        public virtual string NombreZona { get; set; }
        public virtual int IdeUbigeo { get; set; }
        public virtual string TipoNacionalidad { get; set; }
       
        public virtual IList<EstudioPostulante> Estudios { get; set; }

        public virtual string TipoSalario { get; set; }
        public virtual string TipoDisponibilidadTrabajo { get; set; }
        public virtual string TipoDisponibilidadHorario { get; set; }
        public virtual string TipoHorario { get; set;}
        public virtual string IndicadorReubicarseInterior { get; set; }
        public virtual string IndicadorParientesCHSP { get; set; }
        public virtual string TipoParienteSede { get; set; }
        public virtual string ParienteNombre { get; set; }
        public virtual string ParienteCargo { get; set; }
        public virtual string DescripcionOtroMedio { get; set; }
        public virtual string TipoComoSeEntero { get; set; }

        public Postulante()
        {
            Estudios = new List<EstudioPostulante>();
        }

        public virtual void agregarEstudio(EstudioPostulante estudioPostulante)
        {
            estudioPostulante.Postulante = this;
            Estudios.Add(estudioPostulante);
        }
     

    }
}