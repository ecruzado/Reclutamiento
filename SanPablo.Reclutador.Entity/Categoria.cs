using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Categoria : BaseEntity
    {
        public virtual int IDECATEGORIA { get; set; }
        public virtual string ORDENIMPRESION { get; set; }
        public virtual string NOMCATEGORIA { get; set; }
        public virtual string DESCCATEGORIA { get; set; }
        public virtual string TIPCATEGORIA { get; set; }
        public virtual string ESTACTIVO { get; set; }
        public virtual string USRCREACION { get; set; }
        public virtual string FECCREACION { get; set; }
        public virtual string USRMODIFICA { get; set; }
        public virtual string FECMODIFICA { get; set; }


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