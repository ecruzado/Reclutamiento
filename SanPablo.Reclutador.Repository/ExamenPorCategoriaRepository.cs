namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;

    public class ExamenPorCategoriaRepository : Repository<ExamenPorCategoria>, IExamenPorCategoriaRepository
    {
        public ExamenPorCategoriaRepository(ISession session)
            : base(session)
        {
        }

        public List<ExamenPorCategoria> ListarExamenesPorCategoria(int idePostulante, int ideSolicitud, string tipoSolicitud, int ideSede)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                IDataReader drExamenes;
                ExamenPorCategoria objExamenPorCategoria;
                List<ExamenPorCategoria> listaExamenes;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_OBTENER_EXAMENES_POST");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;

                lspcmd.Parameters.Add("p_idPostulante", OracleType.Int32).Value = idePostulante;
                lspcmd.Parameters.Add("p_idSolicitud", OracleType.Int32).Value = ideSolicitud;
                lspcmd.Parameters.Add("p_idTipoSol", OracleType.VarChar).Value = tipoSolicitud;
                lspcmd.Parameters.Add("p_ideSede", OracleType.Int32).Value = ideSede;
                lspcmd.Parameters.Add("p_cuRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                drExamenes = (OracleDataReader)lspcmd.ExecuteReader();
                objExamenPorCategoria = null;
                listaExamenes = new List<ExamenPorCategoria>();


                while (drExamenes.Read())
                {
                    objExamenPorCategoria = new ExamenPorCategoria();
                    objExamenPorCategoria.Examen = new Examen();
                    objExamenPorCategoria.Categoria = new Categoria();

                    objExamenPorCategoria.IdeExamenxCategoria = Convert.ToInt32(drExamenes["IDEEXAMENXCATEGORIA"]);
                    objExamenPorCategoria.Categoria.IDECATEGORIA = Convert.ToInt32(drExamenes["IDECATEGORIA"]);
                    objExamenPorCategoria.Examen.IdeExamen = Convert.ToInt32(drExamenes["IDEEXAMEN"]);
                    objExamenPorCategoria.Examen.NomExamen = Convert.ToString(drExamenes["NOMEXAMEN"]);
                    objExamenPorCategoria.Examen.DescExamen = Convert.ToString(drExamenes["DESCEXAMEN"]);
                    objExamenPorCategoria.Examen.TipExamen = Convert.ToString(drExamenes["TIPEXAMEN"]);
                    objExamenPorCategoria.Examen.TipExamenDes = Convert.ToString(drExamenes["TIPOEXAMEN"]);
                    objExamenPorCategoria.Categoria.ORDENIMPRESION = Convert.ToInt32(drExamenes["ORDENIMPRESION"]);
                    objExamenPorCategoria.Categoria.NOMCATEGORIA = Convert.ToString(drExamenes["NOMCATEGORIA"]);
                    objExamenPorCategoria.Categoria.DESCCATEGORIA = Convert.ToString(drExamenes["DESCCATEGORIA"]);
                    objExamenPorCategoria.Categoria.TIPCATEGORIA = Convert.ToString(drExamenes["TIPCATEGORIA"]);
                    objExamenPorCategoria.Categoria.TIEMPO = Convert.ToInt32(drExamenes["TIEMPO"]);
                    
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