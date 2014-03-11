

namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;

    public class OportunidadLaboralViewModel
    {
        public Postulante postulante { get; set; }
        public Sede sede { get; set; }
        public SolReqPersonal solReqPersonal { get; set; }
        public OportunidadLaboral oportunidadLaboral { get; set; }

        public List<Sede> listaSede { get; set; }
        public List<DetalleGeneral> listaHorario { get; set; }
        public List<SolicitudNuevoCargo> listaCargos { get; set; }

       
       

    }
}