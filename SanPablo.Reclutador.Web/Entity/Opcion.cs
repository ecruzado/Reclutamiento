﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Opcion : BaseEntity
    {
        public virtual int CodigoOpcion { get; set; }
        public virtual Opcion OpcionPadre { get; set; }
        public virtual string NombreOpcion { get; set; }
        public virtual string TipoOpcion { get; set; }
        public virtual string Titulo { get; set; }
        public virtual string Ruta { get; set; }
        public virtual string EstadoRegistro { get; set; }
        public virtual IList<Rol> Roles { get; set; }

        public Opcion()
        {
            Roles = new List<Rol>();
        }
        public virtual void AgregarRoles (Rol rol)
        {
            rol.Opciones.Add(this);
            Roles.Add(rol);
        }
    }
}