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
        public string Accion { get; set; }
        public bool redirecciona { get; set; }
        public int IdSol { get; set; }
        
        public string Iuser { get; set; }
        public string Puser { get; set; }
    }
}