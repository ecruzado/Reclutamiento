namespace SanPablo.Reclutador.Repository.Interface
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

    public interface IRolOpcionRepository : IRepository<RolOpcion>
    {

        int EliminaOpcion(int idRol, int idOpcion);
        List<MenuItem> GetMenu(int idRol);
        List<MenuPadre> GetMenuPadre(int idRol);
        
    }
}