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

    }
}