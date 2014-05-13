using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Examen : BaseEntity
    {
        public virtual int IdeExamen { get; set; }
        public virtual int IdeSede { get; set; }
        public virtual string EstRegistro { get; set; }
        public virtual string NomExamen { get; set; }
        public virtual string DescExamen { get; set; }
        public virtual string TipExamen { get; set; }
        public virtual string TipExamenDes { get; set; }
        
        public virtual string EstActivo { get; set; }

        public virtual string DesEstado { get; set; }

        public virtual string UsrCreacion { get; set; }
        public virtual DateTime FecCreacion { get; set; }
        public virtual string UsrModificacion { get; set; }
       
        public virtual DateTime FecModificacion { get; set; }
        public virtual int Duracion { get; set; }
        public virtual Categoria Categoria { get; set; }
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


        public virtual float TiempoTotal { get; set; }
    }
}