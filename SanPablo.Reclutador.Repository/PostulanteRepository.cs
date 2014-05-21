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


    
    using System.Data;
    

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
                    lobOportunidadLaboral.TipoHorario = (ldrOportunidadLaboral["TIPPUESTO"] == null ? "" : Convert.ToString(ldrOportunidadLaboral["TIPPUESTO"]));

                    var idcargo = ldrOportunidadLaboral["IDECARGO"];
                    if (idcargo != null && idcargo!="") 
                    {
                        lobOportunidadLaboral.IdeCargo = Convert.ToInt32(ldrOportunidadLaboral["IDECARGO"]);
                    }else
	                {
                        lobOportunidadLaboral.IdeCargo=0;
	                }
                   
                    
                    lobOportunidadLaboral.IdeSede = (ldrOportunidadLaboral["IDESEDE"]==null?0:Convert.ToInt32(ldrOportunidadLaboral["IDESEDE"]));
                    lobOportunidadLaboral.SedeDes = (ldrOportunidadLaboral["DESSEDE"]==null?"":Convert.ToString(ldrOportunidadLaboral["DESSEDE"]));
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
            string Observacion = null;

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
                lspcmd.Parameters.Add("p_cObcervacion", OracleType.VarChar, 500).Direction = ParameterDirection.Output;
                
                

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
                Observacion = (lspcmd.Parameters["p_cObcervacion"].Value == null ? "" : Convert.ToString(lspcmd.Parameters["p_cObcervacion"].Value));

                    
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
                SolReqPersonal.ObservacionPublica = Observacion;

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
        public OportunidadLaboral ValidaPostulacion(OportunidadLaboral obj)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            OportunidadLaboral objOportunidad = new OportunidadLaboral();

            int retorno = 0;
            string mensaje = "";
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
                lspcmd.Parameters.Add("p_Mensaje", OracleType.VarChar,2000).Direction = ParameterDirection.Output;

                lspcmd.ExecuteNonQuery();

                retorno = (lspcmd.Parameters["p_retorno"].Value == null ? 0 : Convert.ToInt32(lspcmd.Parameters["p_retorno"].Value));
                mensaje = (lspcmd.Parameters["p_Mensaje"].Value == null ? "" : Convert.ToString(lspcmd.Parameters["p_Mensaje"].Value));


                objOportunidad.retorno = retorno;
                objOportunidad.mensaje = mensaje;

                return objOportunidad;
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
        /// Validacion de seleccion de postulante
        /// </summary>
        /// <param name="obj">objeto del tipo ReclutamientoPersona</param>
        /// <returns>retorna un valor de tipo string</returns>
        public string ValidaSeleccion(ReclutamientoPersona obj)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            string retorno = "";

            try
            {

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_VALIDA_SELECCION");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_nIdPostulante", OracleType.Number).Value = obj.IdePostulante;
                lspcmd.Parameters.Add("p_nIdSede", OracleType.Number).Value = obj.IdSede;
                
                lspcmd.Parameters.Add("p_cRpta", OracleType.VarChar,10).Direction = ParameterDirection.Output;


                lspcmd.ExecuteNonQuery();

                retorno = (lspcmd.Parameters["p_cRpta"].Value == null ? "" : Convert.ToString(lspcmd.Parameters["p_cRpta"].Value));

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
        /// actualiza estado del postulante
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateEstadoPostulante(ReclutamientoPersona obj)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            
            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CAMBIA_ESTADO_POST");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_nidregistro", OracleType.Number).Value = obj.IdeReclutaPersona;
                lspcmd.Parameters.Add("p_ccodestadopost", OracleType.VarChar).Value = obj.EstPostulante;
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


        /// <summary>
        /// obtiene los pustulantes para una determinada solicitud del ranking
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<ReclutamientoPersona> GetPostulantesRanking(ReclutamientoPersona obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader ldrReclutamientoPersona;
                ReclutamientoPersona lobReclutamientoPersona;
                List<ReclutamientoPersona> llstReclutamientoPersona;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_OBTIENE_POSTULANTES_RANKING");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

             
                lspcmd.Parameters.Add("p_nidsol", OracleType.Number).Value = obj.IdeSol;
                lspcmd.Parameters.Add("p_ctipsol", OracleType.VarChar).Value = obj.TipSol;
                 lspcmd.Parameters.Add("p_capepaterno", OracleType.VarChar).Value = obj.ApePaterno;
                 lspcmd.Parameters.Add("p_capematerno", OracleType.VarChar).Value = obj.ApeMaterno;
                 lspcmd.Parameters.Add("p_nombre", OracleType.VarChar).Value = obj.Nombre;
                 lspcmd.Parameters.Add("p_cEstado", OracleType.VarChar).Value = obj.EstPostulante;
                lspcmd.Parameters.Add("p_cretval", OracleType.Cursor).Direction = ParameterDirection.Output;

                ldrReclutamientoPersona = (OracleDataReader)lspcmd.ExecuteReader();
                lobReclutamientoPersona = null;
                llstReclutamientoPersona = new List<ReclutamientoPersona>();


                while (ldrReclutamientoPersona.Read())
                {

                    lobReclutamientoPersona = new ReclutamientoPersona();
                    lobReclutamientoPersona.IdePostulante = (ldrReclutamientoPersona["IDEPOSTULANTE"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDEPOSTULANTE"]));
                    lobReclutamientoPersona.IdeReclutaPersona = (ldrReclutamientoPersona["IDERECLUTAPERSONA"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDERECLUTAPERSONA"]));
                    lobReclutamientoPersona.IdeSol = (ldrReclutamientoPersona["IDESOL"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDESOL"]));
                    lobReclutamientoPersona.TipSol = (ldrReclutamientoPersona["TIPSOL"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["TIPSOL"]));
                    lobReclutamientoPersona.IdeCargo = (ldrReclutamientoPersona["IDECARGO"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDECARGO"]));
                    lobReclutamientoPersona.EstActivo = (ldrReclutamientoPersona["ESTACTIVO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["ESTACTIVO"]));
                    lobReclutamientoPersona.EstPostulante = (ldrReclutamientoPersona["ESTPOSTULANTE"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["ESTPOSTULANTE"]));
                    lobReclutamientoPersona.IndContactado = (ldrReclutamientoPersona["INDCONTACTADO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["INDCONTACTADO"]));

                    lobReclutamientoPersona.Evaluacion = (ldrReclutamientoPersona["EVALUACION"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["EVALUACION"]));
                    lobReclutamientoPersona.PtoTotal = (ldrReclutamientoPersona["PTOTOTAL"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["PTOTOTAL"]));
                    lobReclutamientoPersona.Comentario = (ldrReclutamientoPersona["COMENTARIO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["COMENTARIO"]));
                    lobReclutamientoPersona.TipPuesto = (ldrReclutamientoPersona["TIPPUESTO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["TIPPUESTO"]));
                    lobReclutamientoPersona.IdSede = (ldrReclutamientoPersona["IDSEDE"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDSEDE"]));


                    lobReclutamientoPersona.Apellidos = (ldrReclutamientoPersona["APELLIDOS"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["APELLIDOS"]));
                    lobReclutamientoPersona.Nombres = (ldrReclutamientoPersona["NOMBRES"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["NOMBRES"]));
                    lobReclutamientoPersona.DesEstadoPostulante = (ldrReclutamientoPersona["DESESTADOPOSTULANTE"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["DESESTADOPOSTULANTE"]));
                    
                    lobReclutamientoPersona.FonoFijo = (ldrReclutamientoPersona["TELEFONO_FIJO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["TELEFONO_FIJO"]));
                    lobReclutamientoPersona.FonoMovil = (ldrReclutamientoPersona["TELEFONO_MOVIL"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["TELEFONO_MOVIL"]));
                    lobReclutamientoPersona.EvalPostulante = (ldrReclutamientoPersona["NUMERO_EVALUACION"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["NUMERO_EVALUACION"]));
                    lobReclutamientoPersona.PostulacionParalelo = (ldrReclutamientoPersona["POSTULACIONES"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["POSTULACIONES"]));
                    

                    llstReclutamientoPersona.Add(lobReclutamientoPersona);
                }
                ldrReclutamientoPersona.Close();
                return llstReclutamientoPersona;
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
        /// obtiene los pustulantes preseleccionados
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<ReclutamientoPersona> GetPostulantesPreseleccionado(ReclutamientoPersona obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader ldrReclutamientoPersona;
                ReclutamientoPersona lobReclutamientoPersona;
                List<ReclutamientoPersona> llstReclutamientoPersona;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_POSTULANTE_PRESELEC");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;


                lspcmd.Parameters.Add("p_nidsol", OracleType.Number).Value = obj.IdeSol;
                lspcmd.Parameters.Add("p_ctipsol", OracleType.VarChar).Value = obj.TipSol;
                lspcmd.Parameters.Add("p_cretval", OracleType.Cursor).Direction = ParameterDirection.Output;

                ldrReclutamientoPersona = (OracleDataReader)lspcmd.ExecuteReader();
                lobReclutamientoPersona = null;
                llstReclutamientoPersona = new List<ReclutamientoPersona>();


                while (ldrReclutamientoPersona.Read())
                {

                    lobReclutamientoPersona = new ReclutamientoPersona();
                    lobReclutamientoPersona.IdePostulante = (ldrReclutamientoPersona["IDEPOSTULANTE"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDEPOSTULANTE"]));
                    lobReclutamientoPersona.IdeReclutaPersona = (ldrReclutamientoPersona["IDERECLUTAPERSONA"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDERECLUTAPERSONA"]));
                    lobReclutamientoPersona.IdeSol = (ldrReclutamientoPersona["IDESOL"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDESOL"]));
                    lobReclutamientoPersona.TipSol = (ldrReclutamientoPersona["TIPSOL"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["TIPSOL"]));
                    lobReclutamientoPersona.IdeCargo = (ldrReclutamientoPersona["IDECARGO"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDECARGO"]));
                    lobReclutamientoPersona.EstActivo = (ldrReclutamientoPersona["ESTACTIVO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["ESTACTIVO"]));
                    lobReclutamientoPersona.EstPostulante = (ldrReclutamientoPersona["ESTPOSTULANTE"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["ESTPOSTULANTE"]));
                    lobReclutamientoPersona.IndContactado = (ldrReclutamientoPersona["INDCONTACTADO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["INDCONTACTADO"]));

                    lobReclutamientoPersona.Evaluacion = (ldrReclutamientoPersona["EVALUACION"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["EVALUACION"]));
                    lobReclutamientoPersona.PtoTotal = (ldrReclutamientoPersona["PTOTOTAL"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["PTOTOTAL"]));
                    lobReclutamientoPersona.Comentario = (ldrReclutamientoPersona["COMENTARIO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["COMENTARIO"]));
                    lobReclutamientoPersona.TipPuesto = (ldrReclutamientoPersona["TIPPUESTO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["TIPPUESTO"]));
                    lobReclutamientoPersona.IdSede = (ldrReclutamientoPersona["IDSEDE"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDSEDE"]));


                    lobReclutamientoPersona.Apellidos = (ldrReclutamientoPersona["APELLIDOS"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["APELLIDOS"]));
                    lobReclutamientoPersona.Nombres = (ldrReclutamientoPersona["NOMBRES"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["NOMBRES"]));
                    lobReclutamientoPersona.DesEstadoPostulante = (ldrReclutamientoPersona["DESESTADOPOSTULANTE"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["DESESTADOPOSTULANTE"]));

                    lobReclutamientoPersona.FonoFijo = (ldrReclutamientoPersona["TELEFONO_FIJO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["TELEFONO_FIJO"]));
                    lobReclutamientoPersona.FonoMovil = (ldrReclutamientoPersona["TELEFONO_MOVIL"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["TELEFONO_MOVIL"]));
                    lobReclutamientoPersona.EvalPostulante = (ldrReclutamientoPersona["NUMERO_EVALUACION"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["NUMERO_EVALUACION"]));


                    
                    lobReclutamientoPersona.IndAprobacion = (ldrReclutamientoPersona["INDAPROB"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["INDAPROB"]));
                    
                    llstReclutamientoPersona.Add(lobReclutamientoPersona);
                }
                ldrReclutamientoPersona.Close();
                return llstReclutamientoPersona;
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
        /// obtiene a los postulantes seleccionados y contratados
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<ReclutamientoPersona> GetPostulantesSeleccionados(ReclutamientoPersona obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader ldrReclutamientoPersona;
                ReclutamientoPersona lobReclutamientoPersona;
                List<ReclutamientoPersona> llstReclutamientoPersona;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_POSTULANTE_SELECCIONADOS");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;


                lspcmd.Parameters.Add("p_nidsol", OracleType.Number).Value = obj.IdeSol;
                lspcmd.Parameters.Add("p_ctipsol", OracleType.VarChar).Value = obj.TipSol;
                lspcmd.Parameters.Add("p_cretval", OracleType.Cursor).Direction = ParameterDirection.Output;

                ldrReclutamientoPersona = (OracleDataReader)lspcmd.ExecuteReader();
                lobReclutamientoPersona = null;
                llstReclutamientoPersona = new List<ReclutamientoPersona>();


                while (ldrReclutamientoPersona.Read())
                {

                    lobReclutamientoPersona = new ReclutamientoPersona();
                    lobReclutamientoPersona.IdePostulante = (ldrReclutamientoPersona["IDEPOSTULANTE"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDEPOSTULANTE"]));
                    lobReclutamientoPersona.IdeReclutaPersona = (ldrReclutamientoPersona["IDERECLUTAPERSONA"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDERECLUTAPERSONA"]));
                    lobReclutamientoPersona.IdeSol = (ldrReclutamientoPersona["IDESOL"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDESOL"]));
                    lobReclutamientoPersona.TipSol = (ldrReclutamientoPersona["TIPSOL"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["TIPSOL"]));
                    lobReclutamientoPersona.IdeCargo = (ldrReclutamientoPersona["IDECARGO"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDECARGO"]));
                    lobReclutamientoPersona.EstActivo = (ldrReclutamientoPersona["ESTACTIVO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["ESTACTIVO"]));
                    lobReclutamientoPersona.EstPostulante = (ldrReclutamientoPersona["ESTPOSTULANTE"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["ESTPOSTULANTE"]));
                    lobReclutamientoPersona.IndContactado = (ldrReclutamientoPersona["INDCONTACTADO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["INDCONTACTADO"]));

                    lobReclutamientoPersona.Evaluacion = (ldrReclutamientoPersona["EVALUACION"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["EVALUACION"]));
                    lobReclutamientoPersona.PtoTotal = (ldrReclutamientoPersona["PTOTOTAL"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["PTOTOTAL"]));
                    lobReclutamientoPersona.Comentario = (ldrReclutamientoPersona["COMENTARIO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["COMENTARIO"]));
                    lobReclutamientoPersona.TipPuesto = (ldrReclutamientoPersona["TIPPUESTO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["TIPPUESTO"]));
                    lobReclutamientoPersona.IdSede = (ldrReclutamientoPersona["IDSEDE"] == null ? 0 : Convert.ToInt32(ldrReclutamientoPersona["IDSEDE"]));


                    lobReclutamientoPersona.Apellidos = (ldrReclutamientoPersona["APELLIDOS"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["APELLIDOS"]));
                    lobReclutamientoPersona.Nombres = (ldrReclutamientoPersona["NOMBRES"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["NOMBRES"]));
                    lobReclutamientoPersona.DesEstadoPostulante = (ldrReclutamientoPersona["DESESTADOPOSTULANTE"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["DESESTADOPOSTULANTE"]));

                    lobReclutamientoPersona.FonoFijo = (ldrReclutamientoPersona["TELEFONO_FIJO"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["TELEFONO_FIJO"]));
                    lobReclutamientoPersona.FonoMovil = (ldrReclutamientoPersona["TELEFONO_MOVIL"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["TELEFONO_MOVIL"]));
                    lobReclutamientoPersona.EvalPostulante = (ldrReclutamientoPersona["NUMERO_EVALUACION"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["NUMERO_EVALUACION"]));


                    
                    lobReclutamientoPersona.IndAprobacion = (ldrReclutamientoPersona["INDAPROB"] == null ? "" : Convert.ToString(ldrReclutamientoPersona["INDAPROB"]));
                    
                    

                    llstReclutamientoPersona.Add(lobReclutamientoPersona);
                }
                ldrReclutamientoPersona.Close();
                return llstReclutamientoPersona;
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
        /// obtiene los datos del cv del postulante
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable getDataCvPostulante(Postulante obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            Postulante objPostulante = new Postulante();

            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CV_POSTULANTE");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                if (obj.IdePostulante > 0)
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdePostulante;
                }
                else
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = 0;
                }


                lspcmd.Parameters.Add("p_Rpta", OracleType.Cursor).Direction = ParameterDirection.Output;
               
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

        /// <summary>
        /// obtiene los daatos del nivel academico del postulante
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable getDataCvNivelAcademico(Postulante obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            Postulante objPostulante = new Postulante();

            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CV_NIVEL_ACADEMICO");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                if (obj.IdePostulante > 0)
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdePostulante;
                }
                else
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = 0;
                }


                lspcmd.Parameters.Add("p_Rpta", OracleType.Cursor).Direction = ParameterDirection.Output;

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

        /// <summary>
        /// obtiene las experiencias del postulante
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable getDataCvExperiencias(Postulante obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            Postulante objPostulante = new Postulante();

            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CV_EXPERIENCIA");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                if (obj.IdePostulante > 0)
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdePostulante;
                }
                else
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = 0;
                }


                lspcmd.Parameters.Add("p_Rpta", OracleType.Cursor).Direction = ParameterDirection.Output;

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


        /// <summary>
        /// Obtiene los Conocimientos de ofimatica
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable getDataCvConOfimatica(Postulante obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            Postulante objPostulante = new Postulante();

            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CV_CONOFIMATICA");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                if (obj.IdePostulante > 0)
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdePostulante;
                }
                else
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = 0;
                }


                lspcmd.Parameters.Add("p_Rpta", OracleType.Cursor).Direction = ParameterDirection.Output;

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

        /// <summary>
        /// obtiene los conocimientos de idiomas
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable getDataCvConIdiomas(Postulante obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            Postulante objPostulante = new Postulante();

            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CV_CONIDIOMA");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                if (obj.IdePostulante > 0)
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdePostulante;
                }
                else
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = 0;
                }


                lspcmd.Parameters.Add("p_Rpta", OracleType.Cursor).Direction = ParameterDirection.Output;

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


        /// <summary>
        /// obtiene los otros conocimientos
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable getDataCvConOtros(Postulante obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            Postulante objPostulante = new Postulante();

            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CV_OTROSCONOCIMIENTOS");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                if (obj.IdePostulante > 0)
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdePostulante;
                }
                else
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = 0;
                }


                lspcmd.Parameters.Add("p_Rpta", OracleType.Cursor).Direction = ParameterDirection.Output;

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

        /// <summary>
        /// obtiene los parientes
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable getDataCvParientes(Postulante obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            Postulante objPostulante = new Postulante();

            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CV_PARIENTES");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                if (obj.IdePostulante > 0)
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdePostulante;
                }
                else
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = 0;
                }


                lspcmd.Parameters.Add("p_Rpta", OracleType.Cursor).Direction = ParameterDirection.Output;

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

        /// <summary>
        /// obtiene las discapacidades del postulante
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable getDataCvDiscapacidad(Postulante obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            Postulante objPostulante = new Postulante();

            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CV_DISCAPACIDAD");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                if (obj.IdePostulante > 0)
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdePostulante;
                }
                else
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = 0;
                }


                lspcmd.Parameters.Add("p_Rpta", OracleType.Cursor).Direction = ParameterDirection.Output;

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


        /// <summary>
        /// Obtiene los cargos publicados
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<OportunidadLaboral> GetCargosPublicados(OportunidadLaboral obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader ldrOportunidadLaboral;
                OportunidadLaboral lobOportunidadLaboral;
                List<OportunidadLaboral> llstOportunidadLaboral;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_OBTIENE_CARGOS_PUBLICADOS");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_nIdSede", OracleType.Number).Value = obj.IdeSede;
                lspcmd.Parameters.Add("p_crpta", OracleType.Cursor).Direction = ParameterDirection.Output;

                ldrOportunidadLaboral = (OracleDataReader)lspcmd.ExecuteReader();
                lobOportunidadLaboral = null;
                llstOportunidadLaboral = new List<OportunidadLaboral>();


                while (ldrOportunidadLaboral.Read())
                {

                    lobOportunidadLaboral = new OportunidadLaboral();
                    lobOportunidadLaboral.IdeCargo = Convert.ToInt32(ldrOportunidadLaboral["IDECARGO"]);
                    lobOportunidadLaboral.NombreCargo = Convert.ToString(ldrOportunidadLaboral["NOMCARGO"]);
                    
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


        public List<PostulanteBDReporte> ListaPostulantesBDReporte(PostulanteBDReporte postulante)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            string cFechaDesde = String.Format("{0:dd/MM/yyyy}", postulante.FechaDesde);
            string cFechaHasta = String.Format("{0:dd/MM/yyyy}", postulante.FechaHasta);

            try
            {

                IDataReader drListaPostulantesBD;
                PostulanteBDReporte postulanteBD;
                List<PostulanteBDReporte> listaPostulantes;
                postulanteBD = null;
                listaPostulantes = new List<PostulanteBDReporte>();

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_REQUERIMIENTOS.SP_REPORTE_POSTULANTESBD");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_nombreCargo", OracleType.VarChar).Value = postulante.Cargo;
                lspcmd.Parameters.Add("p_areaEstudio", OracleType.VarChar).Value = postulante.AreaEstudio;
                lspcmd.Parameters.Add("p_rangoSalario", OracleType.VarChar).Value = postulante.RangoSalarial;
                lspcmd.Parameters.Add("p_departamento", OracleType.Number).Value = postulante.IdeDepartamento;
                lspcmd.Parameters.Add("p_provincia", OracleType.Number).Value = postulante.IdeProvincia;
                lspcmd.Parameters.Add("p_distrito", OracleType.Number).Value = postulante.IdeDistrito;
                lspcmd.Parameters.Add("p_fecDesde", OracleType.VarChar).Value = cFechaDesde;
                lspcmd.Parameters.Add("p_fecHasta", OracleType.VarChar).Value = cFechaHasta;
                lspcmd.Parameters.Add("p_edadInicio", OracleType.Number).Value = postulante.EdadInicio;
                lspcmd.Parameters.Add("p_edadFin", OracleType.Number).Value = postulante.EdadFin;
                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                drListaPostulantesBD = (OracleDataReader)lspcmd.ExecuteReader();
                
                while (drListaPostulantesBD.Read())
                {

                    postulanteBD = new PostulanteBDReporte();
                    postulanteBD.IdePostulante = Convert.ToInt32(drListaPostulantesBD["IDEPOSTULANTE"]);
                    postulanteBD.FechaRegistro = String.Format("{0:dd/MM/yyyy}", drListaPostulantesBD["FECCREACION"]);
                    postulanteBD.Departamento = Convert.ToString(drListaPostulantesBD["DEPARTAMENTO"]);
                    postulanteBD.Provincia = Convert.ToString(drListaPostulantesBD["PROVINCIA"]);
                    postulanteBD.Distrito = Convert.ToString(drListaPostulantesBD["DISTRITO"]);
                    postulanteBD.NombreCompleto = Convert.ToString(drListaPostulantesBD["NOMBREAPELLIDO"]);
                    postulanteBD.TelefonoContacto = Convert.ToString(drListaPostulantesBD["TELEFONO"]);
                    postulanteBD.Email = Convert.ToString(drListaPostulantesBD["CORREO"]);
                    postulanteBD.Cargo = Convert.ToString(drListaPostulantesBD["CARGO"]);
                    postulanteBD.Edad = Convert.ToInt32(drListaPostulantesBD["EDAD"]);
                    postulanteBD.TipoEstudio = Convert.ToString(drListaPostulantesBD["TIPOESTUDIO"]);
                    postulanteBD.AreaEstudio = Convert.ToString(drListaPostulantesBD["TIPOAREA"]);
                    postulanteBD.RangoSalarial = Convert.ToString(drListaPostulantesBD["RANGOSALARIO"]);
                    listaPostulantes.Add(postulanteBD);
                }
                drListaPostulantesBD.Close();

                return listaPostulantes;
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


        public DataTable DtPostulantesBDReporte(PostulanteBDReporte postulante)
        {

            string cFechaDesde = String.Format("{0:dd/MM/yyyy}", postulante.FechaDesde);
            string cFechaHasta = String.Format("{0:dd/MM/yyyy}", postulante.FechaHasta);

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_REQUERIMIENTOS.SP_REPORTE_POSTULANTESBD");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_nombreCargo", OracleType.VarChar).Value = postulante.Cargo;
                lspcmd.Parameters.Add("p_areaEstudio", OracleType.VarChar).Value = postulante.AreaEstudio;
                lspcmd.Parameters.Add("p_rangoSalario", OracleType.VarChar).Value = postulante.RangoSalarial;
                lspcmd.Parameters.Add("p_departamento", OracleType.Number).Value = postulante.IdeDepartamento;
                lspcmd.Parameters.Add("p_provincia", OracleType.Number).Value = postulante.IdeProvincia;
                lspcmd.Parameters.Add("p_distrito", OracleType.Number).Value = postulante.IdeDistrito;
                lspcmd.Parameters.Add("p_fecDesde", OracleType.VarChar).Value = cFechaDesde;
                lspcmd.Parameters.Add("p_fecHasta", OracleType.VarChar).Value = cFechaHasta;
                lspcmd.Parameters.Add("p_edadInicio", OracleType.Number).Value = postulante.EdadInicio;
                lspcmd.Parameters.Add("p_edadFin", OracleType.Number).Value = postulante.EdadFin;
                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataAdapter daResultado = new OracleDataAdapter(lspcmd);
                DataTable dtResultado = new DataTable();

                daResultado.Fill(dtResultado);
                daResultado.Dispose();
                return dtResultado;

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


        public List<ReportePostulantePotencial> ListaPostulantesPotenciales(ReportePostulantePotencial postulante)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            string cFechaDesde = String.Format("{0:dd/MM/yyyy}", postulante.FechaDesde);
            string cFechaHasta = String.Format("{0:dd/MM/yyyy}", postulante.FechaHasta);

            try
            {

                IDataReader drListaPostulantesPotenciales;
                ReportePostulantePotencial postulantePotencial;
                List<ReportePostulantePotencial> listaPostulantes;
                postulantePotencial = null;
                listaPostulantes = new List<ReportePostulantePotencial>();

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_REQUERIMIENTOS.SP_REPORTE_POST_POTENCIAL");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_ideCargo", OracleType.Number).Value = postulante.IdeCargo;
                lspcmd.Parameters.Add("p_areaEstudio", OracleType.VarChar).Value = postulante.AreaEstudio;
                lspcmd.Parameters.Add("p_rangoSalario", OracleType.VarChar).Value = postulante.RangoSalarial;
                lspcmd.Parameters.Add("p_ideSede", OracleType.Number).Value = postulante.IdeSede;
                lspcmd.Parameters.Add("p_ideDependencia", OracleType.Number).Value = postulante.IdeDependencia;
                lspcmd.Parameters.Add("p_ideDepartamento", OracleType.Number).Value = postulante.IdeDepartamento;
                lspcmd.Parameters.Add("p_ideArea", OracleType.Number).Value = postulante.IdeArea;
                lspcmd.Parameters.Add("p_fecDesde", OracleType.VarChar).Value = cFechaDesde;
                lspcmd.Parameters.Add("p_fecHasta", OracleType.VarChar).Value = cFechaHasta;
                lspcmd.Parameters.Add("p_edadInicio", OracleType.Number).Value = postulante.EdadInicio;
                lspcmd.Parameters.Add("p_edadFin", OracleType.Number).Value = postulante.EdadFin;
                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                drListaPostulantesPotenciales = (OracleDataReader)lspcmd.ExecuteReader();

                while (drListaPostulantesPotenciales.Read())
                {

                    postulantePotencial = new ReportePostulantePotencial();
                    postulantePotencial.IdeReclutaPersona = Convert.ToInt32(drListaPostulantesPotenciales["IDERECLUTAPERSONA"]);
                    postulantePotencial.IdePostulante = Convert.ToInt32(drListaPostulantesPotenciales["IDEPOSTULANTE"]);
                    postulantePotencial.FechaPostulantePotencial = String.Format("{0:dd/MM/yyyy}", drListaPostulantesPotenciales["FECPOTENCIAL"]);
                    postulantePotencial.Sede = Convert.ToString(drListaPostulantesPotenciales["NOMBRESEDE"]);
                    postulantePotencial.Dependencia = Convert.ToString(drListaPostulantesPotenciales["NOMBREDEPENDENCIA"]);
                    postulantePotencial.Departamento = Convert.ToString(drListaPostulantesPotenciales["NOMBREDEPARTAMENTO"]);
                    postulantePotencial.Area = Convert.ToString(drListaPostulantesPotenciales["NOMBREAREA"]);
                    postulantePotencial.NombreCompleto = Convert.ToString(drListaPostulantesPotenciales["NOMBREPOSTULANTE"]);
                    postulantePotencial.Cargo = Convert.ToString(drListaPostulantesPotenciales["NOMBRECARGO"]);
                    postulantePotencial.TelefonoContacto = Convert.ToString(drListaPostulantesPotenciales["TELEFONO"]);
                    postulantePotencial.Email = Convert.ToString(drListaPostulantesPotenciales["CORREO"]);
                    postulantePotencial.Edad = Convert.ToInt32(drListaPostulantesPotenciales["EDAD"]);
                    postulantePotencial.TipoEstudio = Convert.ToString(drListaPostulantesPotenciales["TIPOEDUCACION"]);
                    postulantePotencial.PuntajeCV = Convert.ToInt32(drListaPostulantesPotenciales["PTJECV"]);
                    postulantePotencial.PuntajeSeleccion = Convert.ToInt32(drListaPostulantesPotenciales["PTJESELECCION"]);
                    postulantePotencial.AreaEstudio = Convert.ToString(drListaPostulantesPotenciales["TIPOAREA"]);
                    postulantePotencial.RangoSalarial = Convert.ToString(drListaPostulantesPotenciales["RANGOSALARIO"]);
                    listaPostulantes.Add(postulantePotencial);
                }
                drListaPostulantesPotenciales.Close();

                return listaPostulantes;
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

        public DataTable DtReportePostulantesPotencial(ReportePostulantePotencial postulante)
        {

            string cFechaDesde = String.Format("{0:dd/MM/yyyy}", postulante.FechaDesde);
            string cFechaHasta = String.Format("{0:dd/MM/yyyy}", postulante.FechaHasta);

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_REQUERIMIENTOS.SP_REPORTE_POST_POTENCIAL");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_ideCargo", OracleType.Number).Value = postulante.IdeCargo;
                lspcmd.Parameters.Add("p_areaEstudio", OracleType.VarChar).Value = postulante.AreaEstudio;
                lspcmd.Parameters.Add("p_rangoSalario", OracleType.VarChar).Value = postulante.RangoSalarial;
                lspcmd.Parameters.Add("p_ideSede", OracleType.Number).Value = postulante.IdeSede;
                lspcmd.Parameters.Add("p_ideDependencia", OracleType.Number).Value = postulante.IdeDependencia;
                lspcmd.Parameters.Add("p_ideDepartamento", OracleType.Number).Value = postulante.IdeDepartamento;
                lspcmd.Parameters.Add("p_ideArea", OracleType.Number).Value = postulante.IdeArea;
                lspcmd.Parameters.Add("p_fecDesde", OracleType.VarChar).Value = cFechaDesde;
                lspcmd.Parameters.Add("p_fecHasta", OracleType.VarChar).Value = cFechaHasta;
                lspcmd.Parameters.Add("p_edadInicio", OracleType.Number).Value = postulante.EdadInicio;
                lspcmd.Parameters.Add("p_edadFin", OracleType.Number).Value = postulante.EdadFin;
                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataAdapter daResultado = new OracleDataAdapter(lspcmd);
                DataTable dtResultado = new DataTable();

                daResultado.Fill(dtResultado);
                daResultado.Dispose();
                return dtResultado;

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
        /// obtiene los datos del postulante para la impresion del Cv
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<CvPostulante> ListaCvPostulante(CvPostulante obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            try
            {

                IDataReader drListaCvPostulante;
                CvPostulante objCvPostulante;
                List<CvPostulante> listaCvPostulante;
                objCvPostulante = null;
                listaCvPostulante = new List<CvPostulante>();

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CV_POSTULANTE");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                if (obj.IdCvPostulante > 0)
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdCvPostulante;
                }
                else
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = 0;
                }
                lspcmd.Parameters.Add("p_rpta", OracleType.Cursor).Direction = ParameterDirection.Output;

                drListaCvPostulante = (OracleDataReader)lspcmd.ExecuteReader();

                while (drListaCvPostulante.Read())
                {

                    objCvPostulante = new CvPostulante();


                    if (drListaCvPostulante["CODTIPDOCUMENTO"] != null && drListaCvPostulante["CODTIPDOCUMENTO"] != DBNull.Value)
                    {
                        objCvPostulante.Codtipdocumento = Convert.ToString(drListaCvPostulante["CODTIPDOCUMENTO"]);
                    }

                    if (drListaCvPostulante["DESTIPDOCUMENTO"] != null && drListaCvPostulante["DESTIPDOCUMENTO"] != DBNull.Value)
                    {
                        objCvPostulante.Destipdocumento = Convert.ToString(drListaCvPostulante["DESTIPDOCUMENTO"]);
                    }
                    if (drListaCvPostulante["NUMDOCUMENTO"] != null && drListaCvPostulante["NUMDOCUMENTO"] != DBNull.Value)
                    {
                        objCvPostulante.Numdocumento = Convert.ToString(drListaCvPostulante["NUMDOCUMENTO"]);
                    }
                    if (drListaCvPostulante["APEPATERNO"] != null && drListaCvPostulante["APEPATERNO"] != DBNull.Value)
                    {
                        objCvPostulante.ApePaterno = Convert.ToString(drListaCvPostulante["APEPATERNO"]);
                        //objCvPostulante.Apepaterno = Convert.ToString(drListaCvPostulante["APEPATERNO"]);
                    }
                    if (drListaCvPostulante["APEMATERNO"] != null && drListaCvPostulante["APEMATERNO"] != DBNull.Value)
                    {
                        objCvPostulante.ApeMaterno = Convert.ToString(drListaCvPostulante["APEMATERNO"]);
                        //objCvPostulante.Apematerno = Convert.ToString(drListaCvPostulante["APEMATERNO"]);
                    }
                    if (drListaCvPostulante["PRINOMBRE"] != null && drListaCvPostulante["PRINOMBRE"] != DBNull.Value)
                    {
                        objCvPostulante.Prinombre = Convert.ToString(drListaCvPostulante["PRINOMBRE"]);
                    }
                    if (drListaCvPostulante["SEGNOMBRE"] != null && drListaCvPostulante["SEGNOMBRE"] != DBNull.Value)
                    {
                        objCvPostulante.Segnombre = Convert.ToString(drListaCvPostulante["SEGNOMBRE"]);
                    }
                    if (drListaCvPostulante["CODNACIONALIDAD"] != null && drListaCvPostulante["CODNACIONALIDAD"] != DBNull.Value)
                    {
                        objCvPostulante.Codnacionalidad = Convert.ToString(drListaCvPostulante["CODNACIONALIDAD"]);
                    }

                    if (drListaCvPostulante["DESNACIONALIDAD"] != null && drListaCvPostulante["DESNACIONALIDAD"] != DBNull.Value)
                    {
                        objCvPostulante.Desnacionalidad = Convert.ToString(drListaCvPostulante["DESNACIONALIDAD"]);
                    }

                    if (drListaCvPostulante["FECNACIMIENTO"] != null && drListaCvPostulante["FECNACIMIENTO"] != DBNull.Value)
                    {
                        objCvPostulante.Fecnacimiento = Convert.ToString(drListaCvPostulante["FECNACIMIENTO"]);
                    }
                    if (drListaCvPostulante["CODSEXO"] != null && drListaCvPostulante["CODSEXO"] != DBNull.Value)
                    {
                        objCvPostulante.Codsexo = Convert.ToString(drListaCvPostulante["CODSEXO"]);
                    }

                    if (drListaCvPostulante["DESSEXO"] != null && drListaCvPostulante["DESSEXO"] != DBNull.Value)
                    {
                        objCvPostulante.Dessexo = Convert.ToString(drListaCvPostulante["DESSEXO"]);
                    }
                    if (drListaCvPostulante["CODESTADOCIVIL"] != null && drListaCvPostulante["CODESTADOCIVIL"] != DBNull.Value)
                    {
                        objCvPostulante.Codestadocivil = Convert.ToString(drListaCvPostulante["CODESTADOCIVIL"]);
                    }

                    if (drListaCvPostulante["DESESTADOCIVIL"] != null && drListaCvPostulante["DESESTADOCIVIL"] != DBNull.Value)
                    {
                        objCvPostulante.Desestadocivil = Convert.ToString(drListaCvPostulante["DESESTADOCIVIL"]);
                    }
                    if (drListaCvPostulante["NUMLICENCIA"] != null && drListaCvPostulante["NUMLICENCIA"] != DBNull.Value)
                    {
                        objCvPostulante.Numlicencia = Convert.ToString(drListaCvPostulante["NUMLICENCIA"]);
                    }

                    if (drListaCvPostulante["OBSERVACION"] != null && drListaCvPostulante["OBSERVACION"] != DBNull.Value)
                    {
                        objCvPostulante.Observacion = Convert.ToString(drListaCvPostulante["OBSERVACION"]);
                    }
                    if (drListaCvPostulante["PAIS"] != null && drListaCvPostulante["PAIS"] != DBNull.Value)
                    {
                        objCvPostulante.Pais = Convert.ToString(drListaCvPostulante["PAIS"]);
                    }
                    if (drListaCvPostulante["IDEUBIGEO"] != null && drListaCvPostulante["IDEUBIGEO"] != DBNull.Value)
                    {
                        objCvPostulante.Ideubigeo = Convert.ToString(drListaCvPostulante["IDEUBIGEO"]);
                    }
                    if (drListaCvPostulante["DESDISTRITO"] != null && drListaCvPostulante["DESDISTRITO"] != DBNull.Value)
                    {
                        objCvPostulante.Desdistrito = Convert.ToString(drListaCvPostulante["DESDISTRITO"]);
                    }
                    if (drListaCvPostulante["DESPROVINCIA"] != null && drListaCvPostulante["DESPROVINCIA"] != DBNull.Value)
                    {
                        objCvPostulante.Desprovincia = Convert.ToString(drListaCvPostulante["DESPROVINCIA"]);
                    }
                    if (drListaCvPostulante["DESDEPARTAMENTO"] != null && drListaCvPostulante["DESDEPARTAMENTO"] != DBNull.Value)
                    {
                        objCvPostulante.Desdepartamento = Convert.ToString(drListaCvPostulante["DESDEPARTAMENTO"]);
                    }
                    if (drListaCvPostulante["CORREO"] != null && drListaCvPostulante["CORREO"] != DBNull.Value)
                    {
                        objCvPostulante.Correo = Convert.ToString(drListaCvPostulante["CORREO"]);
                    }
                    if (drListaCvPostulante["TELMOVIL"] != null && drListaCvPostulante["TELMOVIL"] != DBNull.Value)
                    {
                        objCvPostulante.Telmovil = Convert.ToString(drListaCvPostulante["TELMOVIL"]);
                    }
                    if (drListaCvPostulante["TELFIJO"] != null && drListaCvPostulante["TELFIJO"] != DBNull.Value)
                    {
                        objCvPostulante.Telfijo = Convert.ToString(drListaCvPostulante["TELFIJO"]);
                    }

                    if (drListaCvPostulante["REFERENCIA"] != null && drListaCvPostulante["REFERENCIA"] != DBNull.Value)
                    {
                        objCvPostulante.Referencia = Convert.ToString(drListaCvPostulante["REFERENCIA"]);
                    }
                    if (drListaCvPostulante["CODTIPVIA"] != null && drListaCvPostulante["CODTIPVIA"] != DBNull.Value)
                    {
                        objCvPostulante.Codtipvia = Convert.ToString(drListaCvPostulante["CODTIPVIA"]);
                    }
                    if (drListaCvPostulante["DESTIPVIA"] != null && drListaCvPostulante["DESTIPVIA"] != DBNull.Value)
                    {
                        objCvPostulante.Destipvia = Convert.ToString(drListaCvPostulante["DESTIPVIA"]);
                    }
                    if (drListaCvPostulante["NOMVIA"] != null && drListaCvPostulante["NOMVIA"] != DBNull.Value)
                    {
                        objCvPostulante.Nomvia = Convert.ToString(drListaCvPostulante["NOMVIA"]);
                    }
                    if (drListaCvPostulante["NUMDIRECCION"] != null && drListaCvPostulante["NUMDIRECCION"] != DBNull.Value)
                    {
                        objCvPostulante.Numdireccion = Convert.ToString(drListaCvPostulante["NUMDIRECCION"]);
                    }
                    if (drListaCvPostulante["MANZANA"] != null && drListaCvPostulante["MANZANA"] != DBNull.Value)
                    {
                        objCvPostulante.Manzana = Convert.ToString(drListaCvPostulante["MANZANA"]);
                    }

                    if (drListaCvPostulante["BLOQUE"] != null && drListaCvPostulante["BLOQUE"] != DBNull.Value)
                    {
                        objCvPostulante.Bloque = Convert.ToString(drListaCvPostulante["BLOQUE"]);
                    }

                    if (drListaCvPostulante["CODTIPZONA"] != null && drListaCvPostulante["CODTIPZONA"] != DBNull.Value)
                    {
                        objCvPostulante.Codtipzona = Convert.ToString(drListaCvPostulante["CODTIPZONA"]);
                    }
                    if (drListaCvPostulante["DESTIPZONA"] != null && drListaCvPostulante["DESTIPZONA"] != DBNull.Value)
                    {
                        objCvPostulante.Destipzona = Convert.ToString(drListaCvPostulante["DESTIPZONA"]);
                    }
                    if (drListaCvPostulante["NOMZONA"] != null && drListaCvPostulante["NOMZONA"] != DBNull.Value)
                    {
                        objCvPostulante.Nomzona = Convert.ToString(drListaCvPostulante["NOMZONA"]);
                    }
                    if (drListaCvPostulante["INTERIOR"] != null && drListaCvPostulante["INTERIOR"] != DBNull.Value)
                    {
                        objCvPostulante.Interior = Convert.ToString(drListaCvPostulante["INTERIOR"]);
                    }
                    if (drListaCvPostulante["LOTE"] != null && drListaCvPostulante["LOTE"] != DBNull.Value)
                    {
                        objCvPostulante.Lote = Convert.ToString(drListaCvPostulante["LOTE"]);
                    }
                    if (drListaCvPostulante["ETAPA"] != null && drListaCvPostulante["ETAPA"] != DBNull.Value)
                    {
                        objCvPostulante.Etapa = Convert.ToString(drListaCvPostulante["ETAPA"]);
                    }
                   
                                       
                    if (drListaCvPostulante["FOTOPOSTULANTE"]!=null && drListaCvPostulante["FOTOPOSTULANTE"] !=DBNull.Value)
                    {
                        objCvPostulante.Fotopostulante = (byte[])(drListaCvPostulante["FOTOPOSTULANTE"]);
                    }

                    if (drListaCvPostulante["SALARIO"] != null && drListaCvPostulante["SALARIO"] != DBNull.Value)
                    {
                        objCvPostulante.Salario = Convert.ToString(drListaCvPostulante["SALARIO"]);
                    }

                    if (drListaCvPostulante["DISPTRABAJO"] != null && drListaCvPostulante["DISPTRABAJO"] != DBNull.Value)
                    {
                        objCvPostulante.Disptrabajo = Convert.ToString(drListaCvPostulante["DISPTRABAJO"]);
                    }

                    if (drListaCvPostulante["DISPPHORARIO"] != null && drListaCvPostulante["DISPPHORARIO"] != DBNull.Value)
                    {
                        objCvPostulante.Dispphorario = Convert.ToString(drListaCvPostulante["DISPPHORARIO"]);
                    }
                    if (drListaCvPostulante["HORATRABAJO"] != null && drListaCvPostulante["HORATRABAJO"] != DBNull.Value)
                    {
                        objCvPostulante.Horatrabajo = Convert.ToString(drListaCvPostulante["HORATRABAJO"]);
                    }
                    if (drListaCvPostulante["REUBICACION"] != null && drListaCvPostulante["REUBICACION"] != DBNull.Value)
                    {
                        objCvPostulante.Reubicacion = Convert.ToString(drListaCvPostulante["REUBICACION"]);
                    }

                    if (drListaCvPostulante["PARIENTETRAB"] != null && drListaCvPostulante["PARIENTETRAB"] != DBNull.Value)
                    {
                        objCvPostulante.Parientetrab = Convert.ToString(drListaCvPostulante["PARIENTETRAB"]);
                    }

                    if (drListaCvPostulante["PARIENTESEDE"] != null && drListaCvPostulante["PARIENTESEDE"] != DBNull.Value)
                    {
                        objCvPostulante.Parientesede = Convert.ToString(drListaCvPostulante["PARIENTESEDE"]);
                    }
                    if (drListaCvPostulante["PARIENTECARGO"] != null && drListaCvPostulante["PARIENTECARGO"] != DBNull.Value)
                    {
                        objCvPostulante.Parientecargo = Convert.ToString(drListaCvPostulante["PARIENTECARGO"]);
                    }
                    if (drListaCvPostulante["COMOSEENTERO"] != null && drListaCvPostulante["COMOSEENTERO"] != DBNull.Value)
                    {
                        objCvPostulante.Comoseentero = Convert.ToString(drListaCvPostulante["COMOSEENTERO"]);
                    }

                    if (drListaCvPostulante["EDAD"] != null && drListaCvPostulante["EDAD"] != DBNull.Value)
                    {
                        objCvPostulante.Edad = Convert.ToString(drListaCvPostulante["EDAD"]);
                    }
                    if (drListaCvPostulante["DESEDAD"] != null && drListaCvPostulante["DESEDAD"] != DBNull.Value)
                    {
                        objCvPostulante.Desedad = Convert.ToString(drListaCvPostulante["DESEDAD"]);
                    }
                    if (drListaCvPostulante["NOMBRECOMPLETO"] != null && drListaCvPostulante["NOMBRECOMPLETO"] != DBNull.Value)
                    {
                        objCvPostulante.Nombrecompleto = Convert.ToString(drListaCvPostulante["NOMBRECOMPLETO"]);
                    }
                    if (drListaCvPostulante["IDEPOSTULANTE"] != null && drListaCvPostulante["IDEPOSTULANTE"] != DBNull.Value)
                    {
                        objCvPostulante.Idepostulante = Convert.ToString(drListaCvPostulante["IDEPOSTULANTE"]);
                    }

                    if (drListaCvPostulante["DESDIR"] != null && drListaCvPostulante["DESDIR"] != DBNull.Value)
                    {
                        objCvPostulante.Desdir = Convert.ToString(drListaCvPostulante["DESDIR"]);
                    }
                    if (drListaCvPostulante["MODIFICACION"] != null && drListaCvPostulante["MODIFICACION"] != DBNull.Value)
                    {
                        objCvPostulante.Modificacion = Convert.ToString(drListaCvPostulante["MODIFICACION"]);
                    }
                   
                    


                    
                    listaCvPostulante.Add(objCvPostulante);
                }
                drListaCvPostulante.Close();

                return listaCvPostulante;
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
        /// lista de experiencias para el cv
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<CvPostulante> ListaCvExperiencia(CvPostulante obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            try
            {

                IDataReader drListaCvPostulante;
                CvPostulante objCvPostulante;
                List<CvPostulante> listaCvPostulante;
                objCvPostulante = null;
                listaCvPostulante = new List<CvPostulante>();

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CV_EXPERIENCIA");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                if (obj.IdCvPostulante > 0)
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdCvPostulante;
                }
                else
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = 0;
                }
                lspcmd.Parameters.Add("p_rpta", OracleType.Cursor).Direction = ParameterDirection.Output;

                drListaCvPostulante = (OracleDataReader)lspcmd.ExecuteReader();

                while (drListaCvPostulante.Read())
                {

                    objCvPostulante = new CvPostulante();


                    if (drListaCvPostulante["IDEEXPPOSTULANTE"] != null && drListaCvPostulante["IDEEXPPOSTULANTE"] != DBNull.Value)
                    {
                        objCvPostulante.Ideexppostulante = Convert.ToInt32(drListaCvPostulante["IDEEXPPOSTULANTE"]);
                    }

                    if (drListaCvPostulante["NOMEMPRESA"] != null && drListaCvPostulante["NOMEMPRESA"] != DBNull.Value)
                    {
                        objCvPostulante.Nomempresa = Convert.ToString(drListaCvPostulante["NOMEMPRESA"]);
                    }
                    if (drListaCvPostulante["TIEMPOSERVICIO"] != null && drListaCvPostulante["TIEMPOSERVICIO"] != DBNull.Value)
                    {
                        objCvPostulante.Tiemposervicio = Convert.ToString(drListaCvPostulante["TIEMPOSERVICIO"]);
                    }
                    if (drListaCvPostulante["CARGO"] != null && drListaCvPostulante["CARGO"] != DBNull.Value)
                    {
                        objCvPostulante.CargoExp = Convert.ToString(drListaCvPostulante["CARGO"]);
                    }
                    if (drListaCvPostulante["FECTRABAJO"] != null && drListaCvPostulante["FECTRABAJO"] != DBNull.Value)
                    {
                        objCvPostulante.Fectrabajo = Convert.ToString(drListaCvPostulante["FECTRABAJO"]);
                    }
                    if (drListaCvPostulante["FUCNIONES"] != null && drListaCvPostulante["FUCNIONES"] != DBNull.Value)
                    {
                        objCvPostulante.Fucniones = Convert.ToString(drListaCvPostulante["FUCNIONES"]);
                    }
                    if (drListaCvPostulante["MOTIVOCESE"] != null && drListaCvPostulante["MOTIVOCESE"] != DBNull.Value)
                    {
                        objCvPostulante.Motivocese = Convert.ToString(drListaCvPostulante["MOTIVOCESE"]);
                    }

                    if (drListaCvPostulante["NOMREFERENTE"] != null && drListaCvPostulante["NOMREFERENTE"] != DBNull.Value)
                    {
                        objCvPostulante.Nomreferente = Convert.ToString(drListaCvPostulante["NOMREFERENTE"]);
                    }

                    if (drListaCvPostulante["FONOINST"] != null && drListaCvPostulante["FONOINST"] != DBNull.Value)
                    {
                        objCvPostulante.Fonoinst = Convert.ToString(drListaCvPostulante["FONOINST"]);
                    }

                    if (drListaCvPostulante["ANEXOINST"] != null && drListaCvPostulante["ANEXOINST"] != DBNull.Value)
                    {
                        objCvPostulante.Anexoinst = Convert.ToString(drListaCvPostulante["ANEXOINST"]);
                    }

                    if (drListaCvPostulante["CARGOREFERENTE"] != null && drListaCvPostulante["CARGOREFERENTE"] != DBNull.Value)
                    {
                        objCvPostulante.Cargoreferente = Convert.ToString(drListaCvPostulante["CARGOREFERENTE"]);
                    }
                    if (drListaCvPostulante["FONOREFERENTE"] != null && drListaCvPostulante["FONOREFERENTE"] != DBNull.Value)
                    {
                        objCvPostulante.Fonoreferente = Convert.ToString(drListaCvPostulante["FONOREFERENTE"]);
                    }

                    if (drListaCvPostulante["CORREOREFERENTE"] != null && drListaCvPostulante["CORREOREFERENTE"] != DBNull.Value)
                    {
                        objCvPostulante.Correoreferente = Convert.ToString(drListaCvPostulante["CORREOREFERENTE"]);
                    }

                    listaCvPostulante.Add(objCvPostulante);
                }
                drListaCvPostulante.Close();

                return listaCvPostulante;
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
        /// Estudios nivel Academico
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<CvPostulante> ListaCvEstudios(CvPostulante obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            try
            {

                IDataReader drListaCvPostulante;
                CvPostulante objCvPostulante;
                List<CvPostulante> listaCvPostulante;
                objCvPostulante = null;
                listaCvPostulante = new List<CvPostulante>();

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CV_NIVEL_ACADEMICO");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                if (obj.IdCvPostulante > 0)
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdCvPostulante;
                }
                else
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = 0;
                }
                lspcmd.Parameters.Add("p_rpta", OracleType.Cursor).Direction = ParameterDirection.Output;

                drListaCvPostulante = (OracleDataReader)lspcmd.ExecuteReader();

                while (drListaCvPostulante.Read())
                {

                    objCvPostulante = new CvPostulante();


                    if (drListaCvPostulante["IDEESTUDIOSPOSTULANTE"] != null && drListaCvPostulante["IDEESTUDIOSPOSTULANTE"] != DBNull.Value)
                    {
                        objCvPostulante.Ideestudiospostulante = Convert.ToInt32(drListaCvPostulante["IDEESTUDIOSPOSTULANTE"]);
                    }

                    if (drListaCvPostulante["INSTITUCION"] != null && drListaCvPostulante["INSTITUCION"] != DBNull.Value)
                    {
                        objCvPostulante.Institucion = Convert.ToString(drListaCvPostulante["INSTITUCION"]);
                    }
                    if (drListaCvPostulante["AREAESTUDIO"] != null && drListaCvPostulante["AREAESTUDIO"] != DBNull.Value)
                    {
                        objCvPostulante.Areaestudio = Convert.ToString(drListaCvPostulante["AREAESTUDIO"]);
                    }
                    if (drListaCvPostulante["NIVELESTUDIO"] != null && drListaCvPostulante["NIVELESTUDIO"] != DBNull.Value)
                    {
                        objCvPostulante.Nivelestudio = Convert.ToString(drListaCvPostulante["NIVELESTUDIO"]);
                    }
                    if (drListaCvPostulante["NIVELALCANZADO"] != null && drListaCvPostulante["NIVELALCANZADO"] != DBNull.Value)
                    {
                        objCvPostulante.Nivelalcanzado = Convert.ToString(drListaCvPostulante["NIVELALCANZADO"]);
                    }
                    if (drListaCvPostulante["DESDE"] != null && drListaCvPostulante["DESDE"] != DBNull.Value)
                    {
                        objCvPostulante.DesdeEstudio = Convert.ToString(drListaCvPostulante["DESDE"]);
                    }
                    if (drListaCvPostulante["HASTA"] != null && drListaCvPostulante["HASTA"] != DBNull.Value)
                    {
                        objCvPostulante.HastaEstudio = Convert.ToString(drListaCvPostulante["HASTA"]);
                    }

                    if (drListaCvPostulante["FECESTUDIO"] != null && drListaCvPostulante["FECESTUDIO"] != DBNull.Value)
                    {
                        objCvPostulante.Fecestudio = Convert.ToString(drListaCvPostulante["FECESTUDIO"]);
                    }



                    listaCvPostulante.Add(objCvPostulante);
                }
                drListaCvPostulante.Close();

                return listaCvPostulante;
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
        /// lista de conocimientos de ofimatica
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<CvPostulante> ListaCvConocOfimatica(CvPostulante obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            try
            {

                IDataReader drListaCvPostulante;
                CvPostulante objCvPostulante;
                List<CvPostulante> listaCvPostulante;
                objCvPostulante = null;
                listaCvPostulante = new List<CvPostulante>();

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CV_CONOFIMATICA");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                if (obj.IdCvPostulante > 0)
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = obj.IdCvPostulante;
                }
                else
                {
                    lspcmd.Parameters.Add("p_nidpostulante", OracleType.Number).Value = 0;
                }
                lspcmd.Parameters.Add("p_rpta", OracleType.Cursor).Direction = ParameterDirection.Output;

                drListaCvPostulante = (OracleDataReader)lspcmd.ExecuteReader();

                while (drListaCvPostulante.Read())
                {

                    objCvPostulante = new CvPostulante();


                    if (drListaCvPostulante["IDECONOGENPOSTULANTE"] != null && drListaCvPostulante["IDECONOGENPOSTULANTE"] != DBNull.Value)
                    {
                        objCvPostulante.Ideconogenpostulante = Convert.ToInt32(drListaCvPostulante["IDECONOGENPOSTULANTE"]);
                    }

                    if (drListaCvPostulante["TIPO"] != null && drListaCvPostulante["TIPO"] != DBNull.Value)
                    {
                        objCvPostulante.TipoConocimiento = Convert.ToString(drListaCvPostulante["TIPO"]);
                    }
                    if (drListaCvPostulante["DESCRIPCION"] != null && drListaCvPostulante["DESCRIPCION"] != DBNull.Value)
                    {
                        objCvPostulante.DescripcionConocimiento = Convert.ToString(drListaCvPostulante["DESCRIPCION"]);
                    }
                    if (drListaCvPostulante["TIPNIVELCONOCIMIENTO"] != null && drListaCvPostulante["TIPNIVELCONOCIMIENTO"] != DBNull.Value)
                    {
                        objCvPostulante.Tipnivelconocimiento = Convert.ToString(drListaCvPostulante["TIPNIVELCONOCIMIENTO"]);
                    }
                    if (drListaCvPostulante["INDCERTIFICACION"] != null && drListaCvPostulante["INDCERTIFICACION"] != DBNull.Value)
                    {
                        objCvPostulante.IndcertificacionConc = Convert.ToString(drListaCvPostulante["INDCERTIFICACION"]);
                    }

                    


                    listaCvPostulante.Add(objCvPostulante);
                }
                drListaCvPostulante.Close();

                return listaCvPostulante;
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
