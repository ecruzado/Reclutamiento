

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


    public class ReclutamientoPersonaExamenRepository : Repository<ReclutamientoPersonaExamen>, IReclutamientoPersonaExamenRepository
    {
        public ReclutamientoPersonaExamenRepository(ISession session)
            : base(session)
        {
        }

        public List<ReclutamientoPersonaExamen> obtenerEvaluacionesPostulante(int idePostulante, int idReclutaPersona, string usuarioSession)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
               

                IDataReader drEvaluaciones;
                ReclutamientoPersonaExamen objEvaluacion;
                List<ReclutamientoPersonaExamen> listaEvaluaciones;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_REQUERIMIENTOS.SP_GENERAR_EVAL_REQ_POSTUL");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_idePostulante", OracleType.Int32).Value = idePostulante;
                lspcmd.Parameters.Add("p_ideReclutPost", OracleType.Int32).Value = idReclutaPersona;
                lspcmd.Parameters.Add("p_usuarioCreacion", OracleType.VarChar).Value = usuarioSession;
                lspcmd.Parameters.Add("p_cuExamenes", OracleType.Cursor).Direction = ParameterDirection.Output;

                drEvaluaciones = (OracleDataReader)lspcmd.ExecuteReader();
                objEvaluacion = null;
                listaEvaluaciones = new List<ReclutamientoPersonaExamen>();

                while (drEvaluaciones.Read())
                {
                    objEvaluacion = new ReclutamientoPersonaExamen();
                    objEvaluacion.IdeReclutamientoPersonaExamen = Convert.ToInt32(drEvaluaciones["IDERECLUPERSOEXAMEN"]);
                    objEvaluacion.IdeReclutamientoPersona = Convert.ToInt32(drEvaluaciones["IDERECLUTAPERSONA"]);
                    objEvaluacion.IdeEvaluacion = Convert.ToInt32(drEvaluaciones["IDEEVALUACION"]);
                    objEvaluacion.DescripcionExamen = Convert.ToString(drEvaluaciones["NOMEXAMEN"]);
                    objEvaluacion.TipoExamen = Convert.ToString(drEvaluaciones["TIPOEXAMEN"]);
                    objEvaluacion.IdeUsuarioResponsable = Convert.ToInt32(drEvaluaciones["IDUSUARESPONS"]);
                    objEvaluacion.UsuarioResponsable = Convert.ToString(drEvaluaciones["USUARIORESP"]);

                    objEvaluacion.TipoEstadoEvaluacion = Convert.ToString(drEvaluaciones["TIPESTEVALUACION"]);
                    objEvaluacion.EstadoEvaluacion = Convert.ToString(drEvaluaciones["ESTADOEVALUACION"]);

                    var fecEvaluacion = Convert.ToString(drEvaluaciones["FECEVALUACION"]);
                    if (fecEvaluacion.Length > 0)
                    {
                        objEvaluacion.FechaEvaluacion = Convert.ToDateTime(drEvaluaciones["FECEVALUACION"]);
                    }
                    
                    var horaEvaluacion = Convert.ToString(drEvaluaciones["HORAEVALUACION"]);
                    if (horaEvaluacion.Length > 0)
                    {
                        objEvaluacion.HoraEvaluacion = Convert.ToDateTime(drEvaluaciones["HORAEVALUACION"]);
                    }

                    objEvaluacion.NotaFinal = Convert.ToInt32(drEvaluaciones["NOTAFINAL"]);
                    objEvaluacion.ComentarioResultado = Convert.ToString(drEvaluaciones["COMENTARIORESUL"]);
                    
                    listaEvaluaciones.Add(objEvaluacion);
                }

                drEvaluaciones.Close();
                return listaEvaluaciones;
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

        public void calificacionExamen(int ideReclutaPerExamenCat, int idReclutaPersona, string usuarioSession)
        {

            string usuarioModifica = usuarioSession.Length <= 15 ? usuarioSession : usuarioSession.Substring(0, 15); 
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_REQUERIMIENTOS.SP_CALIFICAR_EXAMEN");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_ideReclPerExaCat", OracleType.Int32).Value = ideReclutaPerExamenCat;
                lspcmd.Parameters.Add("p_ideReclutaPersona", OracleType.Int32).Value = idReclutaPersona;
                lspcmd.Parameters.Add("p_usuarioCreacion", OracleType.VarChar).Value = usuarioModifica;
                lspcmd.Parameters.Add("p_retVal", OracleType.Number).Direction = ParameterDirection.Output;

                lspcmd.ExecuteNonQuery();

                int resultado = Convert.ToInt32(lspcmd.Parameters["p_retVal"].Value);

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

        public DataSet ObtenerEvaluacionReporte(int idereclutaPersona, int ideReclutaPersonaExamen)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            try
            {

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_RECUPERAR_EXAMEN");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_ideReclutaPersona", OracleType.Number).Value = idereclutaPersona;
                lspcmd.Parameters.Add("p_iderecluPersExamen", OracleType.Number).Value = ideReclutaPersonaExamen;
                lspcmd.Parameters.Add("dtExamen", OracleType.Cursor).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("dtCategoriaExamen", OracleType.Cursor).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("dtCriterioAlternativa", OracleType.Cursor).Direction = ParameterDirection.Output;

                DataSet ds = new DataSet();
                OracleDataAdapter adapter = new OracleDataAdapter(lspcmd);
                adapter.Fill(ds);
                adapter.Dispose();
                return ds;
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
