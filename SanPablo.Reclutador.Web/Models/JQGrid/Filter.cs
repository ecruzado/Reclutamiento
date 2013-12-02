using System.Collections.Generic;

namespace SanPablo.Reclutador.Web.Models.JQGrid
{
    public class Filter
    {
        public string groupOp { get; set; }

        public List<Rule> rules { get; set; }
    }
}