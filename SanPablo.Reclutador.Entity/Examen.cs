using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Examen : BaseEntity
    {
        public virtual int CodigoExamen { get; set; }
        public virtual Sede Sede { get; set; }
        public virtual string NombreExamen { get; set; }
        public virtual string DescripcionExamen { get; set; }
        public virtual string TipoExamen { get; set; }
        public virtual string EstadoRegistro { get; set; }
        public virtual IList<Categoria> Categorias { get; set; }

        public Examen()
        {
            Categorias = new List<Categoria>();
        }
        public virtual void AgregarCriterios(Categoria categoria)
        {
            categoria.ExamenesCategoria.Add(this);
            Categorias.Add(categoria);
        }
    }
}