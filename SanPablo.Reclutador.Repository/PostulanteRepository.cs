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


        /// <summary>
        /// obtiene la max solicitud del grupo:
        /// si contiene solicitudes de nuevo cargo muestra los datos de la solicitud nuevo cargo
        /// si no muestra las solcitude de reemplazo o ampliacion de cargo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SolReqPersonal GetDatosSolGrupo(OportunidadLaboral obj)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            
            int IdSol = 0;
            int IdArea = 0;
            string CodRangoSal=null;
            string DesRangoSal = null;
            string DesArea = null;
            string DesSede = null;
            string TipSol = null;
            string NombreCargo = null;
            string Funciones = null;

            SolReqPersonal SolReqPersonal = null;
            try
            {

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_OBTIENE_MAX_SOL_GRUPO_CARGO");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_cTipPuesto", OracleType.VarChar).Value = obj.TipoHorario;
                lspcmd.Parameters.Add("p_nIdCargo", OracleType.Number).Value = obj.IdeCargo;
                lspcmd.Parameters.Add("p_nIdSede", OracleType.Number).Value = obj.IdeSede;
                lspcmd.Parameters.Add("p_nIdSol", OracleType.Number).Direction = ParameterDirection.Output;

                lspcmd.Parameters.Add("p_cSedeDes", OracleType.VarChar,500).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("p_cDesRangoSal", OracleType.VarChar,500).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("p_cCodRangoSal", OracleType.VarChar,3).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("p_nIdArea", OracleType.Number).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("p_cDesArea", OracleType.VarChar,500).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("p_cTipSol", OracleType.VarChar, 500).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("p_cNombreCargo", OracleType.VarChar, 500).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("p_cFunciones", OracleType.VarChar, 500).Direction = ParameterDirection.Output;
                
                

                lspcmd.ExecuteNonQuery();

                IdSol = (lspcmd.Parameters["p_nIdSol"].Value==null?0:Convert.ToInt32(lspcmd.Parameters["p_nIdSol"].Value));
                DesSede = (lspcmd.Parameters["p_cSedeDes"].Value == null ? "" : Convert.ToString(lspcmd.Parameters["p_cSedeDes"].Value));
                DesRangoSal = (lspcmd.Parameters["p_cDesRangoSal"].Value == null ? "" : Convert.ToString(lspcmd.Parameters["p_cDesRangoSal"].Value));
                CodRangoSal = (lspcmd.Parameters["p_cCodRangoSal"].Value == null ? "" : Convert.ToString(lspcmd.Parameters["p_cCodRangoSal"].Value));
                IdArea = (lspcmd.Parameters["p_nIdArea"].Value == null ? 0 : Convert.ToInt32(lspcmd.Parameters["p_nIdArea"].Value));
                DesArea = (lspcmd.Parameters["p_cDesArea"].Value == null ? "" : Convert.ToString(lspcmd.Parameters["p_cDesArea"].Value));
                TipSol = (lspcmd.Parameters["p_cTipSol"].Value == null ? "" : Convert.ToString(lspcmd.Parameters["p_cTipSol"].Value));
                NombreCargo = (lspcmd.Parameters["p_cNombreCargo"].Value == null ? "" : Convert.ToString(lspcmd.Parameters["p_cNombreCargo"].Value));
                Funciones = (lspcmd.Parameters["p_cFunciones"].Value == null ? "" : Convert.ToString(lspcmd.Parameters["p_cFunciones"].Value));


                SolReqPersonal = new SolReqPersonal();

                SolReqPersonal.IdeSolReqPersonal = IdSol;
                SolReqPersonal.Sede_des = DesSede;
                SolReqPersonal.TipoRangoSalarioDes = DesRangoSal;
                SolReqPersonal.TipoRangoSalario = CodRangoSal;
                SolReqPersonal.IdeArea = IdArea;
                SolReqPersonal.Area_des = DesArea;
                SolReqPersonal.Tipsol = TipSol;
                SolReqPersonal.nombreCargo = NombreCargo;
                SolReqPersonal.FuncionesCargo = Funciones;

                return SolReqPersonal;
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
        /// Relaliza la validacion del postulante 
        /// valida que el usuario tenfa todos los datos necesarios para postular
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int ValidaPostulacion(OportunidadLaboral obj)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            int retorno = 0;

            try
            {

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_VALIDA_POSTULACION");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_nIPostulante", OracleType.VarChar).Value = obj.IdPostulante;
                lspcmd.Parameters.Add("p_ctippuesto", OracleType.VarChar).Value = obj.TipoHorario;
                lspcmd.Parameters.Add("p_nidcargo", OracleType.VarChar).Value = obj.IdeCargo;
                lspcmd.Parameters.Add("p_nidsede", OracleType.Number).Value = obj.IdeSede;



                lspcmd.Parameters.Add("p_retorno", OracleType.Number).Direction = ParameterDirection.Output;


                lspcmd.ExecuteNonQuery();

                retorno = (lspcmd.Parameters["p_retorno"].Value == null ? 0 : Convert.ToInt32(lspcmd.Parameters["p_retorno"].Value));

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
        /// Relaiza la postulacion
        /// </summary>
        /// <param name="obj"></param>
        public void Postulacion(Postulante obj)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_POSTULACION");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdePostulante;
                lspcmd.Parameters.Add("p_ctippuesto", OracleType.VarChar).Value = obj.TipoPuesto;
                lspcmd.Parameters.Add("p_nidcargo", OracleType.VarChar).Value = obj.IdCargo;
                lspcmd.Parameters.Add("p_nidsede", OracleType.Number).Value = obj.IdSede;
               
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
        /// obtiene las postulaciones del postulante
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<OportunidadLaboral> GetMisPostulaciones(OportunidadLaboral obj)
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
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_MIS_POSTULACIONES");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_nidpostulante", OracleType.VarChar).Value = obj.IdPostulante;
                lspcmd.Parameters.Add("p_cretval", OracleType.Cursor).Direction = ParameterDirection.Output;

                ldrOportunidadLaboral = (OracleDataReader)lspcmd.ExecuteReader();
                lobOportunidadLaboral = null;
                llstOportunidadLaboral = new List<OportunidadLaboral>();


                while (ldrOportunidadLaboral.Read())
                {

                    lobOportunidadLaboral = new OportunidadLaboral();
                    lobOportunidadLaboral.IdeSede = Convert.ToInt32(ldrOportunidadLaboral["IDSEDE"]);
                    lobOportunidadLaboral.SedeDes = (ldrOportunidadLaboral["DESSEDE"] == null ? "" : Convert.ToString(ldrOportunidadLaboral["DESSEDE"]));
                    lobOportunidadLaboral.IdSolocitud = (ldrOportunidadLaboral["IDESOL"] == null ? 0 : Convert.ToInt32(ldrOportunidadLaboral["IDESOL"]));
                    lobOportunidadLaboral.IdeCargo = (ldrOportunidadLaboral["IDECARGO"] == null ? 0 : Convert.ToInt32(ldrOportunidadLaboral["IDECARGO"]));
                    lobOportunidadLaboral.TipoSol = Convert.ToString(ldrOportunidadLaboral["TIPSOL"]);
                    lobOportunidadLaboral.TipoHorario = Convert.ToString(ldrOportunidadLaboral["TIPPUESTO"]);
                    lobOportunidadLaboral.TipoHorarioDes = Convert.ToString(ldrOportunidadLaboral["DES_PUESTO"]);
                    lobOportunidadLaboral.FechaCreacion = Convert.ToDateTime(ldrOportunidadLaboral["FECHAPOSTULACION"]);
                    lobOportunidadLaboral.NombreCargo = Convert.ToString(ldrOportunidadLaboral["NOMBRE"]);
                    lobOportunidadLaboral.FechaExpiracion = Convert.ToDateTime(ldrOportunidadLaboral["FECEXPIRACION"]);

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
