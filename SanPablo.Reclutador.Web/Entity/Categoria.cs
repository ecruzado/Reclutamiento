﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Categoria : BaseEntity
    {
        public virtual int CodigoCategoria { get; set; }
        public virtual string NombreCategoria { get; set; }
        public virtual string DescripcionCategoria { get; set; }
        public virtual string TipoDeCategoria { get; set; }
        public virtual string EstadoDeRegistro { get; set; }
    }
}