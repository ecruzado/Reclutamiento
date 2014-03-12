

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


    public class SolReqPersonalRepository : Repository<SolReqPersonal>, ISolReqPersonalRepository
    {
         public SolReqPersonalRepository(ISession session)
            : base(session)
        { 
        }

        /// <summary>
        /// obtiene la lista de los cargos activos
        /// </summary>
        /// <param name="IdCargo"></param>
        /// <returns></returns>
         public List<Cargo> GetTipCargo(int IdCargo)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
             try
             {
                 IDataReader ldrCargo;
                 Cargo lobCargo;
                 List<Cargo> llstCargo;
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET.FN_GET_CARGO");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                 lspcmd.Parameters.Add("p_nIdCargo", OracleType.Int32).Value = IdCargo;
                 lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;
                 ldrCargo = (OracleDataReader)lspcmd.ExecuteReader();
                 lobCargo = null;
                 llstCargo = new List<Cargo>();


                 while (ldrCargo.Read())
                 {
                     lobCargo = new Cargo();
                     lobCargo.IdeCargo = Convert.ToInt32(ldrCargo["IDECARGO"]);
                     lobCargo.NombreCargo = Convert.ToString(ldrCargo["NOMCARGO"]);
                     llstCargo.Add(lobCargo);
                 }
                 ldrCargo.Close();
                 return llstCargo;
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
        /// Elimina el Reemplazo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
         public int EliminaListaReemplazo(Reemplazo obj) 
         {

             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
             try
             {
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_ELIMINA_REEMPLAZO");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                 lspcmd.Parameters.Add("p_idReemplazo", OracleType.Int32).Value = obj.IdReemplazo;
                 lspcmd.Parameters.Add("p_idSolReq", OracleType.Int32).Value = obj.IdeSolReqPersonal;
                 lspcmd.Parameters.Add("p_idPersona", OracleType.Int32).Value = obj.IdPersona;
                 lspcmd.Parameters.Add("p_cRetVal", OracleType.Int32).Direction = ParameterDirection.Output;
                 lspcmd.ExecuteNonQuery();
                 return Convert.ToInt32(lspcmd.Parameters["p_cRetVal"].Value);
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
         /// obtiene el responsable de realizar una determinada accion
         /// </summary>
         /// <param name="Tipo"></param>
         /// <param name="sede"></param>
         /// <param name="TipoReq"></param>
         /// <returns></returns>
         public SolReqPersonal GetResponsable(string TipoDerivacion, Int32 sede, string TipoReq)
         {

             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
             int IdUsuarioResp = 0;
             int IdRolResp = 0;
             SolReqPersonal SolReqPersonal = null;
             try
             {
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_ASIGNACION_REMPLAZO");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                 lspcmd.Parameters.Add("p_cTipoDerivacion", OracleType.VarChar).Value = TipoDerivacion;
                 lspcmd.Parameters.Add("p_nIdSede", OracleType.Int32).Value = sede;
                 lspcmd.Parameters.Add("p_nTipoReq", OracleType.VarChar).Value = TipoReq;
                 lspcmd.Parameters.Add("p_nIdUsuarioResp", OracleType.Int32).Direction = ParameterDirection.Output;
                 lspcmd.Parameters.Add("p_nIdRol", OracleType.Int32).Direction = ParameterDirection.Output;
                 lspcmd.ExecuteNonQuery();

                 IdUsuarioResp = Convert.ToInt32(lspcmd.Parameters["p_nIdUsuarioResp"].Value);
                 IdRolResp = Convert.ToInt32(lspcmd.Parameters["p_nIdRol"].Value);
                 SolReqPersonal = new SolReqPersonal();

                 SolReqPersonal.idUsuarioResp = IdUsuarioResp;
                 SolReqPersonal.IdRolResp = IdRolResp;

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
         /// Actualiza el log de la solictud con la etapa actual
         /// </summary>
         /// <param name="obj"></param>
         /// <returns></returns>
         public void ActualizaLogSolReq(LogSolReqPersonal obj)
         {

             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             string cFechaSuceso = String.Format("{0:dd/MM/yyyy}", obj.FecSuceso);

             try
             {
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_INSERT_LOG_SOLREQPERSONAL");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                 lspcmd.Parameters.Add("p_nIDESOLREQPERSONAL", OracleType.Number).Value = obj.IdeSolReqPersonal;
                 lspcmd.Parameters.Add("p_cTIPETAPA", OracleType.VarChar).Value = obj.TipEtapa;
                 lspcmd.Parameters.Add("p_cFECSUCESO", OracleType.VarChar).Value = cFechaSuceso;
                 lspcmd.Parameters.Add("p_nIdUsuarioSuceco", OracleType.Number).Value = obj.UsrSuceso;
                 lspcmd.Parameters.Add("p_nIdRolSuceso", OracleType.Number).Value = obj.RolSuceso;
                 lspcmd.Parameters.Add("p_nIdRolResp", OracleType.Number).Value = obj.RolResponsable;
                 lspcmd.Parameters.Add("p_nIdResponble", OracleType.Number).Value = obj.UsResponsable;
                 lspcmd.Parameters.Add("p_observacion", OracleType.VarChar).Value = (obj.Observacion == null ? "" : obj.Observacion);
                 
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
        /// Crea la Solicitud del reemplazo de cargo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
         public Int32 CreaSolicitudReemplazo(SolReqPersonal solReqPersonal, Reemplazo objReemplazo) 
         {

             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
             string cFechaInicio = String.Format("{0:dd/MM/yyyy}", solReqPersonal.Feccreacion);
             
             try
             {
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CREA_SOLREEMPLAZO");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                 lspcmd.Parameters.Add("p_nIdSede", OracleType.Number).Value = solReqPersonal.IdeSede;
                 lspcmd.Parameters.Add("p_nIdeDependencia", OracleType.Number).Value = solReqPersonal.IdeDependencia;
                 lspcmd.Parameters.Add("p_nIdeCargo", OracleType.Number).Value = solReqPersonal.IdeCargo;
                 lspcmd.Parameters.Add("p_nIdeDepartamento", OracleType.Number).Value = solReqPersonal.IdeDepartamento;
                 lspcmd.Parameters.Add("p_nIdeArea", OracleType.Number).Value = solReqPersonal.IdeArea;
                 lspcmd.Parameters.Add("p_cTipVacante", OracleType.VarChar).Value = solReqPersonal.TipVacante;
                 lspcmd.Parameters.Add("p_nNumVacantes", OracleType.Number).Value = solReqPersonal.NumVacantes;
                 lspcmd.Parameters.Add("p_cTipPuesto", OracleType.VarChar).Value = solReqPersonal.TipPuesto;
                 lspcmd.Parameters.Add("p_cObservacion", OracleType.VarChar).Value = solReqPersonal.Observacion;
                 lspcmd.Parameters.Add("p_idUsuarioSuceso", OracleType.Number).Value = solReqPersonal.idUsuarioSuceso;
                 lspcmd.Parameters.Add("p_cDesUsuarioSuceso", OracleType.VarChar).Value = solReqPersonal.UsuarioCreacion;
                 lspcmd.Parameters.Add("p_cFechaSuceso", OracleType.VarChar).Value = cFechaInicio;
                 lspcmd.Parameters.Add("p_cIdRolSuceso", OracleType.Number).Value = solReqPersonal.idRolSuceso;
                 lspcmd.Parameters.Add("p_cCodReemplazo", OracleType.VarChar).Value = objReemplazo.CodGenerado;
                 lspcmd.Parameters.Add("p_cEtapa", OracleType.VarChar).Value = solReqPersonal.TipEtapa;
                 lspcmd.Parameters.Add("p_idUsuarioResp", OracleType.VarChar).Value = solReqPersonal.idUsuarioResp;
                 lspcmd.Parameters.Add("p_idRolResp", OracleType.Number).Value = solReqPersonal.IdRolResp;
                 lspcmd.Parameters.Add("p_cTipSol", OracleType.VarChar).Value = solReqPersonal.Tipsol;
                 lspcmd.Parameters.Add("p_cRetVal", OracleType.Int32).Direction = ParameterDirection.Output;
                 lspcmd.ExecuteNonQuery();
                 return Convert.ToInt32(lspcmd.Parameters["p_cRetVal"].Value);


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
        /// Inserta los nuevos reemplazos en una tabla que funciona como temporal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
         public int InsertTempReemplazo(Reemplazo obj) 
         {

             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             string cFechaInicio = String.Format("{0:dd/MM/yyyy}", obj.FecInicioReemplazo);
             string cFechaFin = String.Format("{0:dd/MM/yyyy}", obj.FecFinalReemplazo);
             string cFechaCreacion = String.Format("{0:dd/MM/yyyy}", obj.FechaCreacion); 
             
             try
             {
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_INSERTA_REEMPLAZO");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                
                 lspcmd.Parameters.Add("p_cApePaterno", OracleType.VarChar).Value = obj.ApePaterno;
                 lspcmd.Parameters.Add("p_cNomBres", OracleType.VarChar).Value = obj.Nombres;
                 lspcmd.Parameters.Add("p_cFecInicioReemplazo", OracleType.VarChar).Value = cFechaInicio;
                 lspcmd.Parameters.Add("p_cFecFinReemplazo", OracleType.VarChar).Value = cFechaFin;
                 lspcmd.Parameters.Add("p_cUsrCreacion", OracleType.VarChar).Value = obj.UsuarioCreacion;
                 lspcmd.Parameters.Add("p_cFecCreacion", OracleType.VarChar).Value = cFechaCreacion;
                 lspcmd.Parameters.Add("p_nIdeSolReqPersonal", OracleType.Int32).Value = obj.IdeSolReqPersonal;

                 lspcmd.Parameters.Add("p_cRetVal", OracleType.Int32).Direction = ParameterDirection.Output;
                 lspcmd.ExecuteNonQuery();
                 return Convert.ToInt32(lspcmd.Parameters["p_cRetVal"].Value);

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
        /// obtiene las solicitudes resultado de la busqueda inicial
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
         public List<SolReqPersonal> GetListaSolReqPersonal(SolReqPersonal obj)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
             try
             {

                 string cFechaIncial = obj.FechaInicioBus == null ? "" : String.Format("{0:dd/MM/yyyy}", obj.FechaInicioBus);
                 string cFechaFinal = obj.FechaFinBus == null ? "" : String.Format("{0:dd/MM/yyyy}", obj.FechaFinBus);

                 IDataReader ldrSolReqPersonal;
                 SolReqPersonal lobSolReqPersonal;
                 List<SolReqPersonal> llstSolReqPersonal;
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET.FN_GET_LISTAREQ2");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                 lspcmd.Parameters.Add("p_nIdCargo", OracleType.Int32).Value = obj.IdeCargo;
                 lspcmd.Parameters.Add("p_nIdDependencia", OracleType.Int32).Value = obj.IdeDependencia;
                 lspcmd.Parameters.Add("p_nIdDepartamento", OracleType.Int32).Value = obj.IdeDepartamento;
                 lspcmd.Parameters.Add("p_nIdArea", OracleType.Int32).Value = obj.IdeArea;
                 lspcmd.Parameters.Add("p_cTipEtapa", OracleType.VarChar).Value = obj.TipEtapa;
                 lspcmd.Parameters.Add("p_cTipResp", OracleType.VarChar).Value = obj.TipResponsable;
                 lspcmd.Parameters.Add("p_cEstado", OracleType.VarChar).Value = obj.TipEstado;
                 lspcmd.Parameters.Add("p_cFecIni", OracleType.VarChar).Value = cFechaIncial;
                 lspcmd.Parameters.Add("p_cFeFin", OracleType.VarChar).Value = cFechaFinal;
                 lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;
                 ldrSolReqPersonal = (OracleDataReader)lspcmd.ExecuteReader();
                 lobSolReqPersonal = null;
                 llstSolReqPersonal = new List<SolReqPersonal>();

              

                 while (ldrSolReqPersonal.Read())
                 {
                     lobSolReqPersonal = new SolReqPersonal();
                     lobSolReqPersonal.IdeSolReqPersonal = Convert.ToInt32(ldrSolReqPersonal["IDESOLREQPERSONAL"]);
                     lobSolReqPersonal.CodSolReqPersonal = Convert.ToString(ldrSolReqPersonal["CODSOLREQPERSONAL"]);
                     lobSolReqPersonal.IdeCargo = Convert.ToInt32(ldrSolReqPersonal["IDECARGO"]);
                     lobSolReqPersonal.DesCargo = Convert.ToString(ldrSolReqPersonal["DESCARGO"]);
                     lobSolReqPersonal.IdeDependencia = Convert.ToInt32(ldrSolReqPersonal["IDEDEPENDENCIA"]);
                     lobSolReqPersonal.Dependencia_des = Convert.ToString(ldrSolReqPersonal["DESDEPENDENCIA"]);
                     lobSolReqPersonal.IdeDepartamento = Convert.ToInt32(ldrSolReqPersonal["IDEDEPARTAMENTO"]);
                     lobSolReqPersonal.Departamento_des = Convert.ToString(ldrSolReqPersonal["DESDEPARTAMENTO"]);
                     lobSolReqPersonal.IdeArea = Convert.ToInt32(ldrSolReqPersonal["IDEAREA"]);
                     lobSolReqPersonal.Area_des = Convert.ToString(ldrSolReqPersonal["DESAREA"]);

                     lobSolReqPersonal.NumVacantes = Convert.ToInt32(ldrSolReqPersonal["NUMVACANTES"]);
                     lobSolReqPersonal.CantPostulante = Convert.ToInt32(ldrSolReqPersonal["POSTULANTE"]);
                     lobSolReqPersonal.CantPreSelec = Convert.ToInt32(ldrSolReqPersonal["PRESELECCIONADOS"]);
                     lobSolReqPersonal.CantEvaluados = Convert.ToInt32(ldrSolReqPersonal["EVALUADOS"]);
                     lobSolReqPersonal.CantSeleccionados = Convert.ToInt32(ldrSolReqPersonal["SELECCIONADOS"]);


                     lobSolReqPersonal.idRolSuceso = Convert.ToInt32(ldrSolReqPersonal["ROL"]);
                     lobSolReqPersonal.DesRolSuceso = Convert.ToString(ldrSolReqPersonal["DESROL"]);

                     lobSolReqPersonal.TipEstado = Convert.ToString(ldrSolReqPersonal["ESTADO"]);
                     lobSolReqPersonal.TipEtapa = Convert.ToString(ldrSolReqPersonal["TIPETAPA"]);

                     var fecPublicacion = Convert.ToString(ldrSolReqPersonal["FECPUBLICACION"]);
                     if (fecPublicacion.Length>0)
                     {
                         lobSolReqPersonal.FecPublicacion = Convert.ToDateTime(ldrSolReqPersonal["FECPUBLICACION"]);
                     }
                     var fecCreacion = Convert.ToString(ldrSolReqPersonal["FECCREACION"]);
                     if (fecCreacion.Length>0)
                     {
                         lobSolReqPersonal.Feccreacion = Convert.ToDateTime(ldrSolReqPersonal["FECCREACION"]);
                     }

                     var fecExpiracion = Convert.ToString(ldrSolReqPersonal["FECEXPIRACACION"]);
                     if (fecExpiracion.Length>0)
                     {
                         lobSolReqPersonal.FecExpiracacion = Convert.ToDateTime(ldrSolReqPersonal["FECEXPIRACACION"]);
                     }
                     
                     
                     lobSolReqPersonal.NomPersonReemplazo = Convert.ToString(ldrSolReqPersonal["NOMPERSONREEMPLAZO"]);
                     lobSolReqPersonal.FlagPublicado = Convert.ToString(ldrSolReqPersonal["PUBLICADO"]);
                     lobSolReqPersonal.idUsuarioResp = Convert.ToInt32(ldrSolReqPersonal["ID_USUARIO_RESP"]);
                     

                     llstSolReqPersonal.Add(lobSolReqPersonal);
                 }
                 ldrSolReqPersonal.Close();
                 return llstSolReqPersonal;
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
        /// obtiene la lista de reemplazo por solicitud de requerimiento
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
         public List<Reemplazo> GetListaReemplazo(Reemplazo obj)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
             try
             {

                 IDataReader ldrReemplazo;
                 Reemplazo lobReemplazo;
                 List<Reemplazo> llstReemplazo;
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_GET_REEMPLAZO");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                 lspcmd.Parameters.Add("p_nIdeSolReqPersonal", OracleType.Int32).Value = obj.IdeSolReqPersonal;
                 lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;
                 ldrReemplazo = (OracleDataReader)lspcmd.ExecuteReader();
                 lobReemplazo = null;
                 llstReemplazo = new List<Reemplazo>();

                 while (ldrReemplazo.Read())
                 {
            
                     lobReemplazo = new Reemplazo();
                     lobReemplazo.IdReemplazo = Convert.ToInt32(ldrReemplazo["IDREEMPLAZO"]);
                     lobReemplazo.ApePaterno = Convert.ToString(ldrReemplazo["APEPATERNO"]);
                     lobReemplazo.Nombres = Convert.ToString(ldrReemplazo["NOMBRES"]);
                     lobReemplazo.FecInicioReemplazo = Convert.ToDateTime(ldrReemplazo["FECINICIOREEMPLAZO"]);
                     lobReemplazo.FecFinalReemplazo = Convert.ToDateTime(ldrReemplazo["FECFINREEMPLAZO"]);
                     lobReemplazo.IdeSolReqPersonal = Convert.ToInt32(ldrReemplazo["IDESOLREQPERSONAL"]);
                     lobReemplazo.IdPersona = Convert.ToInt32(ldrReemplazo["IDPERSONA"]);
                     
                     llstReemplazo.Add(lobReemplazo);
                 }
                 ldrReemplazo.Close();
                 return llstReemplazo;
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

         #region LISTAR 
         public List<CompetenciaRequerimiento> ListaCompetencias(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            
             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_COMPETENCIAREMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var competencia = new CompetenciaRequerimiento();
                 var listCompetencias = new List<CompetenciaRequerimiento>();
  
                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         competencia = new CompetenciaRequerimiento();
                         competencia.IdeCompetenciaRequerimiento = Convert.ToInt32(lector["IDECOMPETENCIASOLREQ"]);
                         competencia.DescripcionCompetencia = Convert.ToString(lector["DESCRIPCION"]);
                         listCompetencias.Add(competencia);
                     }
                     lector.Close();
                 }
                 
                
                 return listCompetencias;

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


         public List<OfrecemosRequerimiento> ListaOfrecemos(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_OFRECIMIENTO_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var ofrecimiento = new OfrecemosRequerimiento();
                 var listOfrecemos = new List<OfrecemosRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         ofrecimiento = new OfrecemosRequerimiento();
                         ofrecimiento.IdeOfrecemosRequerimiento = Convert.ToInt32(lector["IDEOFRECEMOSSOLREQ"]);
                         ofrecimiento.DescripcionOfrecimiento = Convert.ToString(lector["DESCRIPCION"]);
                         listOfrecemos.Add(ofrecimiento);
                     }
                     lector.Close();
                 }


                 return listOfrecemos;

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

         public List<HorarioRequerimiento> ListaHorarios(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_HORARIO_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var horario = new HorarioRequerimiento();
                 var listaHorario = new List<HorarioRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         horario = new HorarioRequerimiento();
                         horario.IdeHorarioRequerimiento = Convert.ToInt32(lector["IDEHORARIOSOLREQ"]);
                         horario.DescripcionHorario = Convert.ToString(lector["DESCRIPCION"]);
                         horario.PuntajeHorario = Convert.ToInt32(lector["PUNTHORARIO"]);
                         listaHorario.Add(horario);
                     }
                     lector.Close();
                 }

                 return listaHorario;

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

         public List<UbigeoReemplazo> ListaUbigeos(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_UBIGEO_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var ubigeo = new UbigeoReemplazo();
                 var listaHorario = new List<UbigeoReemplazo>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         ubigeo = new UbigeoReemplazo();
                         ubigeo.IdeUbigeoReemplazo = Convert.ToInt32(lector["IDEUBIGEOSOLREQ"]);
                         ubigeo.IdeUbigeo = Convert.ToInt32(lector["IDEUBIGEO"]);
                         ubigeo.Distrito = Convert.ToString(lector["DISTRITO"]);
                         ubigeo.Provincia = Convert.ToString(lector["PROVINCIA"]);
                         ubigeo.Departamento = Convert.ToString(lector["DEPARTAMENT"]);
                         ubigeo.PuntajeUbigeo = Convert.ToInt32(lector["PUNTUBIGEO"]);
                         listaHorario.Add(ubigeo);
                     }
                     lector.Close();
                 }

                 return listaHorario;

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

         public List<CentroEstudioRequerimiento> ListaCentroEstudio(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_CENT_EST_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var centroEstudio = new CentroEstudioRequerimiento();
                 var listaCentroEstudio = new List<CentroEstudioRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         centroEstudio = new CentroEstudioRequerimiento();
                         centroEstudio.IdeCentroEstudioRequerimiento = Convert.ToInt32(lector["IDECENTESTSOLREQ"]);
                         centroEstudio.DescripcionTipoCentroEstudio = Convert.ToString(lector["TIPINST"]);
                         centroEstudio.DescripcionNombreCentroEstudio = Convert.ToString(lector["NOMBINST"]);
                         centroEstudio.PuntajeCentroEstudios = Convert.ToInt32(lector["PUNTACENTROEST"]);
                         listaCentroEstudio.Add(centroEstudio);
                     }
                     lector.Close();
                 }

                 return listaCentroEstudio;

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

         public List<NivelAcademicoRequerimiento> ListaNivelAcademico(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_NIVELACAD_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var nivelAcademico = new NivelAcademicoRequerimiento();
                 var listaNivelAcademico = new List<NivelAcademicoRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         nivelAcademico = new NivelAcademicoRequerimiento();
                         nivelAcademico.IdeNivelAcademicoRequerimiento = Convert.ToInt32(lector["IDENIVELACADESOLREQ"]);
                         nivelAcademico.DescripcionTipoEducacion = Convert.ToString(lector["TIPEDUCACION"]);
                         nivelAcademico.DescripcionAreaEstudio = Convert.ToString(lector["AREAESTUDIO"]);
                         nivelAcademico.DescripcionNivelAlcanzado = Convert.ToString(lector["NIVELALCANZADO"]);
                         nivelAcademico.CicloSemestre = Convert.ToInt32(lector["CICLOSEMESTRE"]);
                         nivelAcademico.PuntajeNivelEstudio = Convert.ToInt32(lector["PUNTNIVESTUDIO"]);
                         listaNivelAcademico.Add(nivelAcademico);
                     }
                     lector.Close();
                 }

                 return listaNivelAcademico;

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

         public List<ConocimientoGeneralRequerimiento> ListaConocimientos(int ideSolicitudReqPersonal, string conocimiento)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_CONOCIMIENTO_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_tipoConocimiento", OracleType.VarChar).Value = conocimiento;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var conocimientoGral = new ConocimientoGeneralRequerimiento();
                 var listaConocimientoGral = new List<ConocimientoGeneralRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         conocimientoGral = new ConocimientoGeneralRequerimiento();
                         conocimientoGral.IdeConocimientoGeneralRequerimiento = Convert.ToInt32(lector["IDECONOGENSOLREQ"]);
                         conocimientoGral.DescripcionConocimientoOfimatica = Convert.ToString(lector["OFIMATICA"]);
                         conocimientoGral.DescripcionNombreOfimatica = Convert.ToString(lector["DESCOFIMATICA"]);
                         conocimientoGral.DescripcionIdioma = Convert.ToString(lector["IDIOMA"]);
                         conocimientoGral.DescripcionConocimientoIdioma = Convert.ToString(lector["CONOIDIOMA"]);
                         conocimientoGral.DescripcionConocimientoGeneral = Convert.ToString(lector["GENERAL"]);
                         conocimientoGral.DescripcionNombreConocimientoGeneral = Convert.ToString(lector["DESCGENERAL"]);
                         conocimientoGral.DescripcionNivelConocimiento = Convert.ToString(lector["NIVELCONO"]);
                         conocimientoGral.PuntajeConocimiento = Convert.ToInt32(lector["PUNTCONOCIMIENTO"]);
                         listaConocimientoGral.Add(conocimientoGral);
                     }
                     lector.Close();
                 }

                 return listaConocimientoGral;

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

         public List<ExperienciaRequerimiento> ListaExperiencia(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_EXPR_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var experiencia = new ExperienciaRequerimiento();
                 var listaExperiencia = new List<ExperienciaRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         experiencia = new ExperienciaRequerimiento();
                         experiencia.IdeExperienciaRequerimiento = Convert.ToInt32(lector["IDEEXPSOLREQ"]);
                         experiencia.DescripcionExperiencia = Convert.ToString(lector["EXPERIENCIA"]);
                         experiencia.CantidadAnhosExperiencia = Convert.ToInt32(lector["CANTANHOEXP"]);
                         experiencia.CantidadMesesExperiencia = Convert.ToInt32(lector["CANTMESESEXP"]);
                         experiencia.PuntajeExperiencia = Convert.ToInt32(lector["PUNTEXPERIENCIA"]);
                         listaExperiencia.Add(experiencia);
                     }
                     lector.Close();
                 }

                 return listaExperiencia;

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

         public List<EvaluacionRequerimiento> ListaEvaluacion(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_EVAL_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var evaluacion = new EvaluacionRequerimiento();
                 var listaEvaluacion = new List<EvaluacionRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         evaluacion = new EvaluacionRequerimiento();
                         evaluacion.IdeEvaluacionRequerimiento = Convert.ToInt32(lector["IDEEVALUACIONSOLREQ"]);
                         evaluacion.DescripcionExamen = Convert.ToString(lector["NOMEXAMEN"]);
                         evaluacion.DescripcionTipoExamen = Convert.ToString(lector["TIPOEXAMEN"]);
                         evaluacion.DescripcionAreaResponsable = Convert.ToString(lector["NOMAREA"]);
                         evaluacion.PuntajeExamen = Convert.ToInt32(lector["PUNTEXAMEN"]);
                         evaluacion.NotaMinimaExamen = Convert.ToInt32(lector["NOTAMINEXAMEN"]);
                         listaEvaluacion.Add(evaluacion);
                     }
                     lector.Close();
                 }

                 return listaEvaluacion;

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

         public List<DiscapacidadRequerimiento> ListaDiscapacidad(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_DISCAP_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var discapacidad = new DiscapacidadRequerimiento();
                 var listaDiscapacidad = new List<DiscapacidadRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         discapacidad = new DiscapacidadRequerimiento();
                         discapacidad.IdeDiscapacidadRequerimiento = Convert.ToInt32(lector["IDEDISCAPASOLREQ"]);
                         discapacidad.DescripcionTipoDiscapacidad = Convert.ToString(lector["DESCDISCAP"]);
                         discapacidad.PuntajeDiscapacidad = Convert.ToInt32(lector["PUNTDISCAPA"]);
                         listaDiscapacidad.Add(discapacidad);
                     }
                     lector.Close();
                 }

                 return listaDiscapacidad;

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
         #endregion

         /// <summary>
         /// inserta la solicitud de Ampliacion de Cargo
         /// </summary>
         public int insertarSolicitudAmpliacion(SolReqPersonal solicitudAmpliacion, int ideUsuarioSuceso, int ideRolSuceso, string etapa, int idRolResponsable, string indArea )
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_INSERTAR_AMPLIACION");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideCargo", OracleType.Int32).Value = solicitudAmpliacion.IdeCargo;
                 cmd.Parameters.Add("p_ideSede", OracleType.Int32).Value = solicitudAmpliacion.IdeSede;
                 cmd.Parameters.Add("p_ideDependencia", OracleType.Int32).Value = solicitudAmpliacion.IdeDependencia;
                 cmd.Parameters.Add("p_ideDepartamento", OracleType.Int32).Value = solicitudAmpliacion.IdeDepartamento;
                 cmd.Parameters.Add("p_ideArea", OracleType.Int32).Value = solicitudAmpliacion.IdeArea;
                 cmd.Parameters.Add("p_numVacantes", OracleType.Int32).Value = solicitudAmpliacion.NumVacantes;
                 cmd.Parameters.Add("p_motivo", OracleType.VarChar).Value = solicitudAmpliacion.Motivo;
                 cmd.Parameters.Add("p_tipoPuesto", OracleType.VarChar).Value = solicitudAmpliacion.TipPuesto;
                 cmd.Parameters.Add("p_observacion", OracleType.VarChar).Value = solicitudAmpliacion.Observacion;
                 cmd.Parameters.Add("p_ideUsuarioSuceso", OracleType.Int32).Value = ideUsuarioSuceso;
                 cmd.Parameters.Add("p_ideRolSuceso", OracleType.Int32).Value = ideRolSuceso;
                 cmd.Parameters.Add("p_cEtapa", OracleType.VarChar).Value = etapa;
                 cmd.Parameters.Add("p_responsableSig", OracleType.Int32).Value = idRolResponsable;
                 cmd.Parameters.Add("p_tipoSolicitud", OracleType.VarChar).Value = solicitudAmpliacion.TipoSolicitud;
                 cmd.Parameters.Add("p_indicArea", OracleType.VarChar).Value = indArea;
                 cmd.Parameters.Add("p_cRetVal", OracleType.Int32).Direction = ParameterDirection.Output;
                 cmd.ExecuteNonQuery();
                 return Convert.ToInt32(cmd.Parameters["p_cRetVal"].Value);

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
         /// determinar responsable de publicacion de acuerdo a requerimiento
         /// </summary>
         public int responsablePublicacion(int ideCargo, int ideSede)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_DETERMINAR_RESPONSABLE");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_idCargo", OracleType.Int32).Value = ideCargo;
                 cmd.Parameters.Add("p_idSede", OracleType.Int32).Value = ideSede;
                 cmd.Parameters.Add("p_idUsuarioResp", OracleType.Int32).Direction = ParameterDirection.Output;
                 cmd.ExecuteNonQuery();
                 return Convert.ToInt32(cmd.Parameters["p_idUsuarioResp"].Value);

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
         /// EnviaSolicitud
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
         public int EnviaSolicitud(SolReqPersonal solReqPersonal)
         {

             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
             string cFechaInicio = String.Format("{0:dd/MM/yyyy}", solReqPersonal.Feccreacion);
             try
             {
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_ENVIA_SOL_REEMPLAZO");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                
                 lspcmd.Parameters.Add("p_nIdeSolReqPersonal", OracleType.Number).Value = solReqPersonal.IdeSolReqPersonal;                
                 lspcmd.Parameters.Add("p_nidUsuarioSuceso", OracleType.Number).Value = solReqPersonal.idUsuarioSuceso;
                 lspcmd.Parameters.Add("p_cDesUsuarioSuceso", OracleType.VarChar).Value = solReqPersonal.UsuarioCreacion;
                 lspcmd.Parameters.Add("p_cFechaSuceso", OracleType.VarChar).Value = cFechaInicio;
                 lspcmd.Parameters.Add("p_cIdRolSuceso", OracleType.Number).Value = solReqPersonal.idRolSuceso;
                 lspcmd.Parameters.Add("p_cEtapa", OracleType.VarChar).Value = solReqPersonal.TipEtapa;
                 lspcmd.Parameters.Add("p_idUsuarioResp", OracleType.Number).Value = solReqPersonal.idUsuarioResp;
                 lspcmd.Parameters.Add("p_idRolResp", OracleType.Number).Value = solReqPersonal.IdRolResp;
                 lspcmd.Parameters.Add("p_nIdeCargo", OracleType.Number).Value = solReqPersonal.IdeCargo;
                 lspcmd.Parameters.Add("p_cRetVal", OracleType.Int32).Direction = ParameterDirection.Output;
                 lspcmd.ExecuteNonQuery();
                 return Convert.ToInt32(lspcmd.Parameters["p_cRetVal"].Value);

                
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
