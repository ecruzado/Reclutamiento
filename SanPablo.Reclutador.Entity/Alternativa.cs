﻿namespace SanPablo.Reclutador.Entity
{
    public class Alternativa : BaseEntity
    {
        public virtual int IdeAlternativa { get; set; }
        public virtual Criterio Criterio { get; set; }
        public virtual string NombreAlternativa { get; set; }
        public virtual int Peso { get; set; }
        public virtual string RutaDeImagen { get; set; }
        public virtual string ESTACTIVO { get; set; }
        public virtual byte[] Image { get; set; }
        public virtual int IdeSede { get; set; }
        public virtual bool IndRespuesta { get; set; }



    }
}