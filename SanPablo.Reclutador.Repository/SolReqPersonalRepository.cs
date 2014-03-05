

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
                 lspcmd.Parameters.Add("p_ideReemplazo", OracleType.Int32).Value = obj.IdReemplazo;
                 lspcmd.Parameters.Add("p_ideSolReq", OracleType.Int32).Value = obj.IdeSolReqPersonal;
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
        /// Crea la Solicitud del reemplazo de cargo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
         public string CreaSolicitudReemplazo(SolReqPersonal solReqPersonal, Reemplazo objReemplazo) 
         {

             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
             string cFechaInicio = String.Format("{0:dd/MM/yyyy}", solReqPersonal.Feccreacion);
             
             try
             {
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_CREA_SOLREEMPLAZO");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                 lspcmd.Parameters.Add("p_nIdSede", OracleType.Int32).Value = solReqPersonal.IdeSede;
                 lspcmd.Parameters.Add("p_nIdeDependencia", OracleType.Int32).Value = solReqPersonal.IdeDependencia;
                 lspcmd.Parameters.Add("p_nIdeCargo", OracleType.Int32).Value = solReqPersonal.IdeCargo;
                 lspcmd.Parameters.Add("p_nIdeDepartamento", OracleType.Int32).Value = solReqPersonal.IdeDepartamento;
                 lspcmd.Parameters.Add("p_nIdeArea", OracleType.Int32).Value = solReqPersonal.IdeArea;
                 lspcmd.Parameters.Add("p_cTipVacante", OracleType.Int32).Value = solReqPersonal.TipVacante;

                 lspcmd.Parameters.Add("p_nNumVacantes", OracleType.Int32).Value = solReqPersonal.NumVacantes;
                 lspcmd.Parameters.Add("p_cTipPuesto", OracleType.VarChar).Value = solReqPersonal.TipPuesto;
                 lspcmd.Parameters.Add("p_cObservacion", OracleType.VarChar).Value = solReqPersonal.Observacion;
                 lspcmd.Parameters.Add("p_cUsuarioCreacion", OracleType.VarChar).Value = solReqPersonal.UsuarioCreacion;
                 lspcmd.Parameters.Add("p_cFechaCreacion", OracleType.VarChar).Value = cFechaInicio;
                 lspcmd.Parameters.Add("p_cCodReemplazo", OracleType.VarChar).Value = objReemplazo.CodGenerado;
                 
                 
                 lspcmd.Parameters.Add("p_cRetVal", OracleType.Int32).Direction = ParameterDirection.Output;
                 lspcmd.ExecuteNonQuery();
                 return Convert.ToString(lspcmd.Parameters["p_cRetVal"].Value);


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
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_INSERTA_REEMPLAZO_TEMP");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                 lspcmd.Parameters.Add("nIdTemp", OracleType.VarChar).Value = obj.CodGenerado;
                 lspcmd.Parameters.Add("nIdReemplazo", OracleType.Int32).Value = obj.IdReemplazo;
                 lspcmd.Parameters.Add("cApePaterno", OracleType.VarChar).Value = obj.ApeMaterno;
                 lspcmd.Parameters.Add("cNomBres", OracleType.VarChar).Value = obj.Nombres;
                 lspcmd.Parameters.Add("cFecInicioReemplazo", OracleType.VarChar).Value = cFechaInicio;
                 lspcmd.Parameters.Add("cFecFinReemplazo", OracleType.VarChar).Value = cFechaFin;
                 lspcmd.Parameters.Add("cUsrCreacion", OracleType.VarChar).Value = obj.UsuarioCreacion;
                 lspcmd.Parameters.Add("cFecCreacion", OracleType.VarChar).Value = cFechaCreacion;
                 lspcmd.Parameters.Add("nIdeSolReqPersonal", OracleType.Int32).Value = obj.IdeSolReqPersonal;
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

                
                 string cFechaIncial = obj.FechaInicioBus==null?"":String.Format("{0:MM/dd/yyyy}", obj.FechaInicioBus);
                 string cFechaFinal = obj.FechaFinBus == null ? "" : String.Format("{0:MM/dd/yyyy}", obj.FechaFinBus);

                 IDataReader ldrSolReqPersonal;
                 SolReqPersonal lobSolReqPersonal;
                 List<SolReqPersonal> llstSolReqPersonal;
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET.FN_GET_LISTAREQ");
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


                     lobSolReqPersonal.IdRol = Convert.ToInt32(ldrSolReqPersonal["ROL"]);
                     lobSolReqPersonal.DesRol = Convert.ToString(ldrSolReqPersonal["DESROL"]);

                     lobSolReqPersonal.TipEstado = Convert.ToString(ldrSolReqPersonal["ESTACTIVO"]);
                     lobSolReqPersonal.TipEtapa = Convert.ToString(ldrSolReqPersonal["TIPETAPA"]);
                     lobSolReqPersonal.FecPublicacion = Convert.ToDateTime(ldrSolReqPersonal["FECPUBLICACION"]);
                     lobSolReqPersonal.Feccreacion = Convert.ToDateTime(ldrSolReqPersonal["FECCREACION"]);
                     lobSolReqPersonal.FecExpiracacion = Convert.ToDateTime(ldrSolReqPersonal["FECEXPIRACACION"]);
                     lobSolReqPersonal.NomPersonReemplazo = Convert.ToString(ldrSolReqPersonal["NOMPERSONREEMPLAZO"]);
                     lobSolReqPersonal.FlagPublicado = Convert.ToString(ldrSolReqPersonal["PUBLICADO"]);
                     

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
                     lobReemplazo.ApeMaterno = Convert.ToString(ldrReemplazo["APEPATERNO"]);
                     lobReemplazo.Nombres = Convert.ToString(ldrReemplazo["NOMBRES"]);
                     lobReemplazo.FecInicioReemplazo = Convert.ToDateTime(ldrReemplazo["FECINICIOREEMPLAZO"]);
                     lobReemplazo.FecFinalReemplazo = Convert.ToDateTime(ldrReemplazo["FECFINREEMPLAZO"]);
                     lobReemplazo.IdeSolReqPersonal = Convert.ToInt32(ldrReemplazo["IDESOLREQPERSONAL"]);
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

    }
}
