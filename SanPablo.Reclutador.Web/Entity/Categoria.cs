using System;
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
        public virtual IList<Examen> ExamenesCategoria { get; set; }
        public virtual IList<SubCategoria> SubCategorias { get; set; }

        public Categoria()
        {
            ExamenesCategoria = new List<Examen>();
            SubCategorias = new List<SubCategoria>();
        }
        public virtual void AgregarExamenesCategoria(Examen examen)
        {
            examen.Categorias.Add(this);
            ExamenesCategoria.Add(examen);
        }
    }
}