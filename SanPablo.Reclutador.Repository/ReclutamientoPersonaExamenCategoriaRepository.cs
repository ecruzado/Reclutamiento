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


    public class ReclutamientoPersonaExamenCategoriaRepository : Repository<ReclutamientoPersonaExamenCategoria>, IReclutamientoPersonaExamenCategoriaRepository
    {
        public ReclutamientoPersonaExamenCategoriaRepository(ISession session)
            : base(session)
        {
        }
        
         public int obtenerIdentificadorCategoria(int idReclutaPersonaExamenCategoria)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
               
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_OBTENER_IDECATEGORIA");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_idReclPerExaCat", OracleType.Int32).Value = idReclutaPersonaExamenCategoria;
                lspcmd.Parameters.Add("p_RetVal", OracleType.Int32).Direction = ParameterDirection.Output;
                lspcmd.ExecuteNonQuery();

                int resultado = Convert.ToInt32(lspcmd.Parameters["p_RetVal"].Value) ;

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

        public bool obtenerExamenesPorCategoria(int idReclutaPersona, string usuarioSession)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
               
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_GENERAR_EXAMEN_CAT_RECL");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_idReclutaPerso", OracleType.Int32).Value = idReclutaPersona;
                lspcmd.Parameters.Add("p_usuario", OracleType.VarChar).Value = usuarioSession;
                lspcmd.Parameters.Add("p_RetVal", OracleType.Int32).Direction = ParameterDirection.Output;
                lspcmd.ExecuteNonQuery();

                int resultado = Convert.ToInt32(lspcmd.Parameters["p_RetVal"].Value) ;

                if(resultado == 0)
                {return false;}
                else
                {
                    return true;
                }
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

        public List<DatosExamenPorCategoria> ListarExamenesPorCategoria(int ideReclutaPersona)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                IDataReader drExamenes;
                DatosExamenPorCategoria objExamenPorCategoria;
                List<DatosExamenPorCategoria> listaExamenes;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_OBTENER_EXAMENES_POST");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_idReclutaPerso", OracleType.Int32).Value = ideReclutaPersona;
                lspcmd.Parameters.Add("p_cuRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                drExamenes = (OracleDataReader)lspcmd.ExecuteReader();
                objExamenPorCategoria = null;
                listaExamenes = new List<DatosExamenPorCategoria>();


                while (drExamenes.Read())
                {
                    objExamenPorCategoria = new DatosExamenPorCategoria();

                    objExamenPorCategoria.IdeReclutamientoPersonaExamenCategoria = Convert.ToInt32(drExamenes["IDERECLPERSOEXAMNCAT"]);
                    objExamenPorCategoria.IdeExamenCategoria = Convert.ToInt32(drExamenes["IDEEXAMENXCATEGORIA"]);
                    objExamenPorCategoria.IdeExamen = Convert.ToInt32(drExamenes["IDEEXAMEN"]);

                    objExamenPorCategoria.IdeCategoria = Convert.ToInt32(drExamenes["IDECATEGORIA"]);
                    objExamenPorCategoria.NombreExamen = Convert.ToString(drExamenes["NOMEXAMEN"]);
                    objExamenPorCategoria.DescripcionExamen = Convert.ToString(drExamenes["DESCEXAMEN"]);
                    objExamenPorCategoria.NombreCategoria = Convert.ToString(drExamenes["NOMCATEGORIA"]);
                    objExamenPorCategoria.OrdenImpresion = Convert.ToInt32(drExamenes["ORDENIMPRESION"]);
                    objExamenPorCategoria.Tiempo = Convert.ToInt32(drExamenes["TIEMPO"]);
                    objExamenPorCategoria.Estado = Convert.ToString(drExamenes["ESTADO"]);

                    listaExamenes.Add(objExamenPorCategoria);
                }

                drExamenes.Close();
                return listaExamenes;
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
