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

    public class RolRepository : Repository<Rol>, IRolRepository
    {
        public RolRepository(ISession session)
            : base(session)
        { 
        }
        /// <summary>
        /// Elimina el trol si no depende de un usuario o sede
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public int EliminaRol(int idRol)
        {


            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_INTRANET.FN_ELIMINA_ROL");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_nIdRol", OracleType.Int32).Value = idRol;
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


    }
}