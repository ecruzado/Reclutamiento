

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
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_ELIMINA_REEMPLAZO");
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


                     lobSolReqPersonal.TipResponsable = Convert.ToString(ldrSolReqPersonal["RESPONSABLE"]);
                     lobSolReqPersonal.NomPersonReemplazo = Convert.ToString(ldrSolReqPersonal["NOMBRESPONSABLE"]);
                     lobSolReqPersonal.TipEstado = Convert.ToString(ldrSolReqPersonal["ESTACTIVO"]);
                     lobSolReqPersonal.TipEtapa = Convert.ToString(ldrSolReqPersonal["TIPETAPA"]);
                     lobSolReqPersonal.FecPublicacion = Convert.ToDateTime(ldrSolReqPersonal["FECPUBLICACION"]);
                     lobSolReqPersonal.Feccreacion = Convert.ToDateTime(ldrSolReqPersonal["FECCREACION"]);

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

    }
}
