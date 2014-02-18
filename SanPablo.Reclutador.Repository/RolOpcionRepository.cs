namespace SanPablo.Reclutador.Repository
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

    public class RolOpcionRepository : Repository<RolOpcion>, IRolOpcionRepository
    {
        public RolOpcionRepository(ISession session)
            : base(session)
        { 
        }

        public int EliminaOpcion(int idRol,int idOpcion)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_INTRANET.FN_ELIMINA_ROL_OPCION");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_nIdRol", OracleType.Int32).Value = idRol;
                cmd.Parameters.Add("p_nIdOp", OracleType.Int32).Value = idOpcion;
                cmd.Parameters.Add("p_rRetVal", OracleType.Number).Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                return Convert.ToInt32(cmd.Parameters[cmd.Parameters.IndexOf("p_rRetVal")].Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lcon.Close();
            }
        }



        /// <summary>
        /// obtiene las opciones del Menu
        /// </summary>
        /// <param name="idMenuItem"></param>
        /// <returns></returns>
        public List<MenuItem> GetMenu(int idRol)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader ldrMenuItem;
                MenuItem lobMenuItem;
                List<MenuItem> llstMenuItem;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.FN_GET_OPCIONESXROL");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_nIdRol", OracleType.Int32).Value = idRol;
                lspcmd.Parameters.Add("p_cretval", OracleType.Cursor).Direction = ParameterDirection.Output;
                ldrMenuItem = (OracleDataReader)lspcmd.ExecuteReader();
                lobMenuItem = null;
                llstMenuItem = new List<MenuItem>();


                while (ldrMenuItem.Read())
                {
                    lobMenuItem = new MenuItem();

                    lobMenuItem.IDOPCIONPADRE = Convert.ToInt32(ldrMenuItem["IDOPCIONPADRE"]);
                    lobMenuItem.IDOPCION = Convert.ToInt32(ldrMenuItem["IDOPCION"]);
                    lobMenuItem.DESCRIPCION = Convert.ToString(ldrMenuItem["DESCRIPCION"]);
                    lobMenuItem.DSCURL = Convert.ToString(ldrMenuItem["DSCURL"]);
                    lobMenuItem.IDROL = Convert.ToInt32(ldrMenuItem["IDROL"]);

                    llstMenuItem.Add(lobMenuItem);

                }
                ldrMenuItem.Close();
                return llstMenuItem;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lcon.Close();
            }
        }


        /// <summary>
        /// obtiene la lista de opciones principales
        /// </summary>
        /// <param name="idRol"></param>
        /// <returns></returns>
        public List<MenuPadre> GetMenuPadre(int idRol)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader ldrMenuPadre;
                MenuPadre lobMenuPadre;
                List<MenuPadre> llstMenuPadre;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.FN_GET_OPCIONESPADREXROL");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_nidrol", OracleType.Int32).Value = idRol;
                lspcmd.Parameters.Add("p_cretval", OracleType.Cursor).Direction = ParameterDirection.Output;
                ldrMenuPadre = (OracleDataReader)lspcmd.ExecuteReader();
                lobMenuPadre = null;
                llstMenuPadre = new List<MenuPadre>();


                while (ldrMenuPadre.Read())
                {
                    lobMenuPadre = new MenuPadre();

                    lobMenuPadre.IDOPCIONPADRE = Convert.ToInt32(ldrMenuPadre["IDOPCIONPADRE"]);
                    lobMenuPadre.DESCRIPCION = Convert.ToString(ldrMenuPadre["DESCRIPCION"]);

                    llstMenuPadre.Add(lobMenuPadre);

                }
                ldrMenuPadre.Close();
                return llstMenuPadre;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lcon.Close();
            }
        }
    }
}