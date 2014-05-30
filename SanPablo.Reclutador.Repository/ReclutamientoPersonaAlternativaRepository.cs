

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


    public class ReclutamientoPersonaAlternativaRepository : Repository<ReclutamientoPersonaAlternativa>, IReclutamientoPersonaAlternativaRepository
    {
        public ReclutamientoPersonaAlternativaRepository(ISession session)
            : base(session)
        {
        }

        public bool guardarRespuesta(int ideReclutaPersona,int ideCriterioSubCategoria, int ideReclutaPersonaExamenCategoria,int ideAlternativa,string usuarioCreacion)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_GUARDAR_ALTERNATIVA");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_ideReclutaPersona", OracleType.Int32).Value = ideReclutaPersona;
                lspcmd.Parameters.Add("p_ideCriterioSubCat", OracleType.Int32).Value = ideCriterioSubCategoria;
                lspcmd.Parameters.Add("p_ideReclPersExaCat", OracleType.Int32).Value = ideReclutaPersonaExamenCategoria;
                lspcmd.Parameters.Add("p_ideAlternativa", OracleType.Int32).Value = ideAlternativa;
                lspcmd.Parameters.Add("p_usuarioCreacion", OracleType.VarChar).Value = usuarioCreacion;
                lspcmd.Parameters.Add("p_RetVal", OracleType.Int32).Direction = ParameterDirection.Output;

                lspcmd.ExecuteNonQuery();

                int resultado = Convert.ToInt32(lspcmd.Parameters[lspcmd.Parameters.IndexOf("p_RetVal")].Value);
                if (resultado == -1)
                { return false; }
                else
                    return true;
                
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
