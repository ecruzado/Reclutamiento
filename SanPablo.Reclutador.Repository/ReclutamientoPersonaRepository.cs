

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


    }
}
