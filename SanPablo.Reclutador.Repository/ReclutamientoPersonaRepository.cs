

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
                
                lspcmd.Parameters.Add("p_nIdResp", OracleType.Number).Value = obj.idResponsable;
                lspcmd.Parameters.Add("p_nIdSuceso", OracleType.Number).Value = obj.idSuceso;
                lspcmd.Parameters.Add("p_nIdRolResp", OracleType.Number).Value = obj.idRolResponsable;
                lspcmd.Parameters.Add("p_nIdRolSuceso", OracleType.Number).Value = obj.idRolSuceso;
               
               


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

        /// <summary>
        /// Realiza la validacion de la solicitud,
        /// si el numero de vacantes es igual al numero de contratdos permite cerrar la solicitud si no muestra 
        /// popup donde debe ingresar un motivo de para cerrar la solicitud
        /// </summary>
        /// <param name="objReCluta"></param>
        /// <returns></returns>
        public string validaFinSolicitud(ReclutamientoPersona obj)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            string retorno = "";

            try
            {

             
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_VALIDA_FIN_SOLICITUD");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_nidsol", OracleType.Number).Value = obj.IdeSol;
                lspcmd.Parameters.Add("p_ctipsol", OracleType.VarChar).Value = obj.TipSol;
                lspcmd.Parameters.Add("p_ctippuesto", OracleType.VarChar).Value = obj.TipPuesto;
                lspcmd.Parameters.Add("p_nidsede", OracleType.Number).Value = obj.IdSede;
                lspcmd.Parameters.Add("p_nidcargo", OracleType.Number).Value = obj.IdeCargo;
                lspcmd.Parameters.Add("p_nvancantes", OracleType.Number).Value = obj.NumVacantes;
                lspcmd.Parameters.Add("p_crpta", OracleType.VarChar,200).Direction = ParameterDirection.Output;


                lspcmd.ExecuteNonQuery();

                retorno = (lspcmd.Parameters["p_crpta"].Value == null ? "" : Convert.ToString(lspcmd.Parameters["p_crpta"].Value));

                return retorno;
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
        /// Recuperar el IdeReclutamientoPersona
        /// </summary>
        /// <param name="idePostulante"></param>
        /// <param name="ideSede"></param>
        /// <param name="estadoPostulante"></param>
        /// <returns></returns>
        public int getIdeReclutaPersona(int idePostulante, int ideSede)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_GET_IDRECLU_PERSON");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_idePostulante", OracleType.Int32).Value = idePostulante;
                lspcmd.Parameters.Add("p_ideSede", OracleType.Int32).Value = ideSede;
                lspcmd.Parameters.Add("p_RetVal", OracleType.Int32).Direction = ParameterDirection.Output;

                lspcmd.ExecuteNonQuery();

                int resultado = Convert.ToInt32(lspcmd.Parameters[lspcmd.Parameters.IndexOf("p_RetVal")].Value);

                return resultado;

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
