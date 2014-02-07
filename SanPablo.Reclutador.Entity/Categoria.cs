using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace SanPablo.Reclutador.Entity
{
    public class Categoria : BaseEntity
    {
        public virtual int IDECATEGORIA { get; set; }
        public virtual int ORDENIMPRESION { get; set; }
        public virtual string NOMCATEGORIA { get; set; }
        public virtual string DESCCATEGORIA { get; set; }
        public virtual string TIPCATEGORIA { get; set; }
        public virtual string ESTACTIVO { get; set; }
        public virtual string USRCREACION { get; set; }
        public virtual DateTime FECCREACION { get; set; }
        public virtual string USRMODIFICA { get; set; }
        public virtual DateTime FECMODIFICA { get; set; }
        public virtual string INSTRUCCIONES { get; set; }
        public virtual string TIPOEJEMPLO { get; set; }
        public virtual byte[] IMAGENEJEMPLO { get; set; }
        public virtual string TEXTOEJEMPLO { get; set; }
        public virtual string TipoCriterio { get; set; }
        public virtual string TIPCATEGORIADES { get; set; }
        public virtual string NOMBREIMAGEN { get; set; }
        
       

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