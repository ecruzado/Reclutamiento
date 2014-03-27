

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


    public class ReclutamientoPersonaRepository : Repository<ReclutamientoPersona>, IReclutamientoPersonaRepository
    {
        public ReclutamientoPersonaRepository(ISession session)
            : base(session)
        {
        }

        public void FinalizaContratacion(ReclutamientoPersona obj)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_FINALIZA_CONTRATACION");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_nidsol", OracleType.Number).Value = obj.IdeSol;
                lspcmd.Parameters.Add("p_ctipsol", OracleType.VarChar).Value = obj.TipSol;
                lspcmd.Parameters.Add("p_ctippuesto", OracleType.VarChar).Value = obj.TipPuesto;
                lspcmd.Parameters.Add("p_nidsede", OracleType.Number).Value = obj.IdSede;
                lspcmd.Parameters.Add("p_nidcargo", OracleType.Number).Value = obj.IdeCargo;
               
                lspcmd.ExecuteNonQuery();

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
