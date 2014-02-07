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
    //using oracle = Oracle.DataAccess.Client;
    //using Oracle.DataAccess.Types;


    public class ExamenRepository : Repository<Examen>, IExamenRepository
    {
        public ExamenRepository(ISession session)
            : base(session)
        { 
        }
        /// <summary>
        /// obtiene el tiempo del examen
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public int getTiempoExamen(int idExamen) {

           
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_INTRANET.FN_DURACIONEXAMEN");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_nidexamen", OracleType.Int32).Value = idExamen;
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
        /// otbtiene los datos para la impresion del examen
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public DataTable getDataRepExamen(int idExamen)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_GETREPEXAMEN");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                if (idExamen > 0)
                {
                    lspcmd.Parameters.Add("P_NIDEXAMEN", OracleType.Number).Value = idExamen;
                }
                else {
                    lspcmd.Parameters.Add("P_NIDEXAMEN", OracleType.Number).Value = 0;
                }

                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataAdapter da = new OracleDataAdapter(lspcmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                da.Dispose();
                return dt;
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