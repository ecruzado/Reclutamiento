﻿namespace SanPablo.Reclutador.Repository.Interface
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Data.OracleClient;
    using System.Configuration;
    using System.Collections;

    using System.Linq;
    using System.Transactions;

    public interface IUsuarioRepository : IRepository<Usuario>
    {
        List<Usuario> GetAnalistaRespoanble(SolReqPersonal obj);
        List<UsuarioVista> GetUsuarioVista(UsuarioVista obj);

        /// <summary>
        /// lista de usuarios para busqueda
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        List<Usuario> listarUsuario(Usuario usuario, int ideSede);
    }
}