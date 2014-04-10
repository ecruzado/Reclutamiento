using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class PdfExamen : BaseEntity
    {

        public virtual int Ideexamen { get; set; }
       
public virtual int Idesede { get; set; }
public virtual string Nomexamen { get; set; }
public virtual string Descexamen { get; set; }
public virtual int Idesubcategoria { get; set; }
public virtual int Idecategoria { get; set; }
public virtual string Nomcategoria { get; set; }
public virtual string Desccategoria { get; set; }
public virtual string Tipcategoria { get; set; }
public virtual string Instrucciones { get; set; }
public virtual string Tipoejemplo { get; set; }
public virtual byte[] Imagenejemplo { get; set; }
public virtual string Textoejemplo { get; set; }
public virtual string Descsubcategoria  { get; set; }
public virtual string Nomsubcategoria { get; set; }
public virtual int Idecriterio { get; set; }
public virtual int Idealternativa { get; set; }
public virtual string Codmod { get; set; }
public virtual string Desmodo { get; set; }
public virtual string Tipcriterio { get; set; }
public virtual string Pregunta { get; set; }
public virtual byte[] Imagencrit { get; set; }
public virtual byte[] Image { get; set; }
public virtual string Alternativa { get; set; }
public virtual string Estactivo { get; set; }
public virtual int Ordensub { get; set; }
public virtual int Ordencrit { get; set; }
public virtual string Tiempocat { get; set; }
public virtual string Timpoexamen { get; set; }



    }
}
