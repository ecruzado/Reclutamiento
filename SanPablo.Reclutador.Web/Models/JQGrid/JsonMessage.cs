using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Models.JQGrid
{
    public class JsonMessage
    {
        public bool Resultado { get; set; }
        public string Mensaje { get; set; }
        public object Objeto { get; set; }
        public int IdDato { get; set; }
    }
}