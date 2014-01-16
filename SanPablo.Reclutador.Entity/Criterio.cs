﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Criterio : BaseEntity
    {
        public virtual int IdeCriterio { get; set; }
        public virtual string Pregunta { get; set; }
        public virtual string DescripcionCriterio { get; set; }
        public virtual string TipoMedicion { get; set; }
        public virtual string TipoCriterio { get; set; }
        public virtual string TipoModo { get; set; }
        public virtual string EstadoRegistro { get; set; }
        public virtual string TipoCalificacion { get; set; }
        
        public virtual int OrdenImpresion { get; set; }

        public virtual IList<SubCategoria> SubCategorias { get; set; }

        public Criterio()
        {
            SubCategorias = new List<SubCategoria>();
        }
        public virtual void AgregarSubCategorias(SubCategoria subCategoria)
        {
            subCategoria.Criterios.Add(this);
            SubCategorias.Add(subCategoria);
        }
    }
}