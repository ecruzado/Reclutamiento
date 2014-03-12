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

    public class PostulanteRepository : Repository<Postulante>, IPostulanteRepository
    {
        public PostulanteRepository(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// obtiene las oportunidades laborales
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<OportunidadLaboral> GetObtieneOpurtunidad(OportunidadLaboral obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
               

                string cFechaIncial = obj.FecInicial == null ? "" : String.Format("{0:dd/MM/yyyy}", obj.FecInicial);
                string cFechaFinal = obj.FecFinal == null ? "" : String.Format("{0:dd/MM/yyyy}", obj.FecFinal);

               
               

                IDataReader ldrOportunidadLaboral;
                OportunidadLaboral lobOportunidadLaboral;
                List<OportunidadLaboral> llstOportunidadLaboral;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_OPORTUNIDAD_LABORAL");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_cTipPuesto", OracleType.VarChar).Value = (obj.TipoHorario==null?"":obj.TipoHorario);
                lspcmd.Parameters.Add("p_nIdCargo", OracleType.Number).Value = obj.IdeCargo;
                lspcmd.Parameters.Add("p_nIdSede", OracleType.Number).Value = obj.IdeSede;
                lspcmd.Parameters.Add("p_cFechaInicio", OracleType.VarChar).Value = cFechaIncial;
                lspcmd.Parameters.Add("p_cFecFin", OracleType.VarChar).Value = cFechaFinal;
                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                ldrOportunidadLaboral = (OracleDataReader)lspcmd.ExecuteReader();
                lobOportunidadLaboral = null;
                llstOportunidadLaboral = new List<OportunidadLaboral>();


                while (ldrOportunidadLaboral.Read())
                {
                    lobOportunidadLaboral = new OportunidadLaboral();
                    lobOportunidadLaboral.TipoHorario = Convert.ToString(ldrOportunidadLaboral["TIPPUESTO"]);
                    lobOportunidadLaboral.IdeCargo = (ldrOportunidadLaboral["IDECARGO"]==null?0:Convert.ToInt32(ldrOportunidadLaboral["IDECARGO"]));
                    lobOportunidadLaboral.IdeSede = (ldrOportunidadLaboral["IDESEDE"]==null?0:Convert.ToInt32(ldrOportunidadLaboral["IDESEDE"]));
                    lobOportunidadLaboral.SedeDes = Convert.ToString(ldrOportunidadLaboral["DESSEDE"]);
                    lobOportunidadLaboral.TipoHorarioDes = Convert.ToString(ldrOportunidadLaboral["DESPUESTO"]);
                    lobOportunidadLaboral.CargoDes = Convert.ToString(ldrOportunidadLaboral["NOMCARGO"]);
                    lobOportunidadLaboral.FecInicial = Convert.ToDateTime(ldrOportunidadLaboral["FECINICIALMAX"]);
                    lobOportunidadLaboral.FecFinal = Convert.ToDateTime(ldrOportunidadLaboral["FECFINALMAX"]);
                    lobOportunidadLaboral.NumVacantes = Convert.ToInt32(ldrOportunidadLaboral["NUMVACANTES"]);
                    
                    llstOportunidadLaboral.Add(lobOportunidadLaboral);
                }
                ldrOportunidadLaboral.Close();
                return llstOportunidadLaboral;
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
