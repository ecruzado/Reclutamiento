

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
                    objEvaluacion.TipoExamen = Convert.ToString(drEvaluaciones["TIPEXAMEN"]);
                    objEvaluacion.DescripcionTipoExamen = Convert.ToString(drEvaluaciones["TIPOEXAMEN"]);
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
                OracleCommand lspcmd = new OracleCommand("PR_REQUERIMIENTOS.SP_RECUPERAR_EXAMEN");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_ideReclutaPersona", OracleType.Number).Value = idereclutaPersona;
                lspcmd.Parameters.Add("p_iderecluPersExamen", OracleType.Number).Value = ideReclutaPersonaExamen;
                lspcmd.Parameters.Add("dtExamen", OracleType.Cursor).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("dtCategoriaExamen", OracleType.Cursor).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("dtCategoriaSubCatego", OracleType.Cursor).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("dtCriterioAlternativa", OracleType.Cursor).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("dtAlternativas", OracleType.Cursor).Direction = ParameterDirection.Output;
               
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





        public ResultadoExamen ObtenerEvaluacionReportePdf(int idereclutaPersona, int ideReclutaPersonaExamen)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            ResultadoExamen resulExamen = new ResultadoExamen();
            IDataReader drExamen;

            try
            {

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_REQUERIMIENTOS.SP_RECUPERAR_EXAMEN");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_ideReclutaPersona", OracleType.Number).Value = idereclutaPersona;
                lspcmd.Parameters.Add("p_iderecluPersExamen", OracleType.Number).Value = ideReclutaPersonaExamen;
                lspcmd.Parameters.Add("dtExamen", OracleType.Cursor).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("dtCategoriaExamen", OracleType.Cursor).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("dtCategoriaSubCatego", OracleType.Cursor).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("dtCriterioAlternativa", OracleType.Cursor).Direction = ParameterDirection.Output;
                lspcmd.Parameters.Add("dtAlternativas", OracleType.Cursor).Direction = ParameterDirection.Output;


                using (IDataReader lector = (OracleDataReader)lspcmd.ExecuteReader())
                {
                    resulExamen = null;
                    resulExamen = new ResultadoExamen();

                    //cursor 1
                    if (lector.Read())
                    {
                        resulExamen.IdeREclutamientoPersonaExamen = Convert.ToInt32(lector["IDERECLUPERSOEXAMEN"]);
                        resulExamen.NombreExamen = Convert.ToString(lector["NOMEXAMEN"]);
                        resulExamen.DescripcionExamen = Convert.ToString(lector["DESCEXAMEN"]);
                        resulExamen.TipoExamen = Convert.ToString(lector["TIPEXAMEN"]);
                        resulExamen.NotaFinal = Convert.ToInt32(lector["NOTAFINAL"]);
                        resulExamen.nombrePostulante = Convert.ToString(lector["NOMBPOSTULANTE"]);

                    }

                    lector.NextResult();
                    resulExamen.Categorias = new List<ResultadoExamenCategoria>();

                    while (lector.Read())
                    {
                        var item = new ResultadoExamenCategoria();
                        item.IdeReclutamientoExamenCategoria = Convert.ToInt32(lector["IDERECLPERSOEXAMNCAT"]);
                        item.IdeReclutamientoPersonaExamen = Convert.ToInt32(lector["IDERECLUPERSOEXAMEN"]);
                        item.NumeroPreguntas = Convert.ToInt32(lector["NROPREGUNTAS"]);
                        item.NotaCategoria = Convert.ToInt32(lector["NOTAEXAMENCATEG"]);
                        item.NombreCategoria = Convert.ToString(lector["NOMCATEGORIA"]);
                        item.DescripcionCategoria = Convert.ToString(lector["DESCCATEGORIA"]);
                        item.Tiempo = Convert.ToInt32(lector["TIEMPO"]);

                        resulExamen.Categorias.Add(item);

                    }

                    lector.NextResult();
                    resulExamen.SubCategorias = new List<ResultadoExamenSubCategoria>();

                    while (lector.Read())
                    {
                        var item = new ResultadoExamenSubCategoria();
                        item.IdeSubCategoria = Convert.ToInt32(lector["IDESUBCATEGORIA"]);
                        item.IdeReclutamientoExamenCategoria = Convert.ToInt32(lector["IDERECLPERSOEXAMNCAT"]);
                        item.IdeReclutamientoPersonaExamen = Convert.ToInt32(lector["IDERECLUPERSOEXAMEN"]);
                        item.NombreSubCategoria = Convert.ToString(lector["NOMSUBCATEGORIA"]);
                        item.DescripcionSubCategoria = Convert.ToString(lector["DESCSUBCATEGORIA"]);
                        item.OrdenImpresion = Convert.ToInt32(lector["ORDENIMPRESION"]);

                        resulExamen.SubCategorias.Add(item);

                    }

                    lector.NextResult();
                    resulExamen.Criterios = new List<ResultadoExamenCriterio>();

                    while (lector.Read())
                    {
                        var item = new ResultadoExamenCriterio();
                        item.IdeReclutaPersonaCriterio = Convert.ToInt32(lector["IDERECLUPERSOCRITERIO"]);
                        item.IdeReclutamientoExamenCategoria = Convert.ToInt32(lector["IDERECLPERSOEXAMNCAT"]);
                        item.IdeCriterioSubCategoria = Convert.ToInt32(lector["IDECRITERIOXSUBCATEGORIA"]);
                        item.IdeSubCategoria = Convert.ToInt32(lector["IDESUBCATEGORIA"]);
                        item.Pregunta = Convert.ToString(lector["PREGUNTA"]);
                        item.TipoModo = Convert.ToString(lector["TIPMODO"]);

                        if (lector["IMAGENCRIT"] != null && lector["IMAGENCRIT"] != DBNull.Value)
                        {
                            item.ImagenCriterio = (byte[])(lector["IMAGENCRIT"]);
                        }

                        item.IndicadorRespuesta = Convert.ToString(lector["INDRESPUESTA"]);
                        item.PuntajeTotal = Convert.ToInt32(lector["PUNTTOTAL"]);

                        resulExamen.Criterios.Add(item);
                    }

                    lector.NextResult();
                    resulExamen.Alternativas = new List<ResultadoExamenAlternativa>();

                    while (lector.Read())
                    {
                        var item = new ResultadoExamenAlternativa();
                        item.IdeReclutaPersonaCriterio = Convert.ToInt32(lector["IDERECLUPERSOCRITERIO"]);
                        item.IdeCriterio = Convert.ToInt32(lector["IDECRITERIO"]);
                        item.IdeCriterioSubCategoria = Convert.ToInt32(lector["IDECRITERIOXSUBCATEGORIA"]);
                        item.Alternativa = Convert.ToString(lector["ALTERNATIVA"]);
                        item.IdeAlternativa = Convert.ToInt32(lector["IDEALTERNATIVA"]);
                        item.Peso = Convert.ToInt32(lector["PESO"]);

                        if (lector["IMAGE"] != null && lector["IMAGE"] != DBNull.Value)
                        {
                            item.ImagenAlternativa = (byte[])(lector["IMAGE"]);
                        }

                        item.Respuesta = Convert.ToString(lector["RESPUESTA"]);

                        resulExamen.Alternativas.Add(item);
                    }

                    lector.Close();

                }
                    
                return resulExamen;
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


        public string existeResultado(int ideReclutaPersonaExamen)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            try
            {

                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_REQUERIMIENTOS.FN_EXISTE_RESULTADO_EXAMEN");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_ideReclutaExamen", OracleType.Number).Value = ideReclutaPersonaExamen;
                lspcmd.Parameters.Add("p_rRetVal", OracleType.VarChar,1).Direction = ParameterDirection.ReturnValue;
                lspcmd.ExecuteNonQuery();

                return Convert.ToString(lspcmd.Parameters[lspcmd.Parameters.IndexOf("p_rRetVal")].Value);
                
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
