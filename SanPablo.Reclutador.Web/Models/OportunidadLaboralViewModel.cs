﻿

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
        public List<Cargo> listaCargos { get; set; }
        public List<OportunidadLaboral> listaOportunidadLaboral { get; set; }

        public string VisualizarCompetenecias { get; set; }
        public string VisualizarOfrecemos { get; set; }
        public string VisualizarIdiomas { get; set; }
        public string VisualizarOfimatica { get; set; }
        public string VisualizarOtrosConocimientos { get; set; }
    }
}