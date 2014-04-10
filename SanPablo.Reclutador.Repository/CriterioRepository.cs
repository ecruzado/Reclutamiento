namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;

    public class CriterioRepository : Repository<Criterio>,ICriterioRepository
    {
        public CriterioRepository(ISession session)
            : base(session)
        { 
        }

        public IList<Criterio> ObtenerListaMarciana(string codigo)
        {
            return new List<Criterio>();
        }

        public ResultadoQuery<Criterio> GetPagingBySql(string sortField, bool ascending, int pageIndex, int pageSize, string where) 
        {
            return null;
        }

         


        /// <summary>
        /// obtiene la lista de criterios
        /// </summary>
        /// <param name="IdeCategoria"></param>
        /// <returns></returns>
        public List<Criterio> ObtenerCriterios(Criterio obj)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader drCriterios;
                Criterio objCriterio;
                List<Criterio> listaCriterios = new List<Criterio>();
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.FN_OBTIENE_CRITERIOS");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_ctipmedicion", OracleType.VarChar).Value = obj.TipoMedicion;
                lspcmd.Parameters.Add("p_cpregunta", OracleType.VarChar).Value = obj.Pregunta;
                lspcmd.Parameters.Add("p_ctipcriterio", OracleType.VarChar).Value = obj.TipoCriterio;
                lspcmd.Parameters.Add("p_cestado", OracleType.VarChar).Value = obj.IndicadorActivo;
                lspcmd.Parameters.Add("p_crpta", OracleType.Cursor).Direction = ParameterDirection.Output;
                drCriterios = (OracleDataReader)lspcmd.ExecuteReader();
                objCriterio = null;
                
                //listaCriterios.criterios = new List<Criterio>();
                
               
                while (drCriterios.Read())
                {
                    objCriterio = new Criterio();

                   
                    objCriterio.IdeCriterio = Convert.ToInt32(drCriterios["IDECRITERIO"]);
                    objCriterio.IndicadorActivo = Convert.ToString(drCriterios["ESTACTIVO"]);
                    objCriterio.Pregunta = Convert.ToString(drCriterios["PREGUNTA"]);
                    objCriterio.TipoMedicion = Convert.ToString(drCriterios["TIPMEDICION"]);
                    objCriterio.TipoModo = Convert.ToString(drCriterios["TIPMODO"]);
                    objCriterio.TipoCriterio = Convert.ToString(drCriterios["TIPCRITERIO"]);
                    objCriterio.TipoCalificacion = Convert.ToString(drCriterios["TIPCALIFICACION"]);

                    objCriterio.TipoCriterioDes = Convert.ToString(drCriterios["DESTIPCRITERIO"]);
                    objCriterio.TipoCalificacionDes = Convert.ToString(drCriterios["DESTIPCALIFICACION"]);
                    objCriterio.TipoMedicionDes = Convert.ToString(drCriterios["DESTIPMEDICION"]);
                    objCriterio.TipoModoDes = Convert.ToString(drCriterios["DESTIPMODO"]);

                    listaCriterios.Add(objCriterio);
                    
                }

                drCriterios.Close();
                return listaCriterios;
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
        /// obtiene los criterios por categoria
        /// </summary>
        /// <param name="IdeCategoria"></param>
        /// <returns></returns>
        public ListaCriterios ObtenerCriteriosPorCategoria(int IdeCategoria)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader drCriterios;
                Criterio  objCriterio;
                ListaCriterios listaCriterios = new ListaCriterios();
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_EXAMEN_CATEGORIA");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_ideCategoria", OracleType.Number).Value = IdeCategoria;
                lspcmd.Parameters.Add("p_cuRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;
                drCriterios = (OracleDataReader)lspcmd.ExecuteReader();
                objCriterio = null;
                listaCriterios.criterios = new List<Criterio>();
                int contador = 0;
                while (drCriterios.Read())
                {
                    objCriterio = new Criterio();

                    objCriterio.numeracion = contador + 1;
                    objCriterio.IdeCriterio = Convert.ToInt32(drCriterios["IDECRITERIO"]);
                    objCriterio.IdeSubCategoria = Convert.ToInt32(drCriterios["IDESUBCATEGORIA"]);
                    objCriterio.IdeCriterioPorSubcategoria = Convert.ToInt32(drCriterios["IDECRITERIOXSUBCATEGORIA"]);
                    objCriterio.NombreSubCategoria = Convert.ToString(drCriterios["NOMSUBCATEGORIA"]);
                    objCriterio.Tiempo = Convert.ToInt32(drCriterios["TIEMPO"]);
                    objCriterio.Pregunta = Convert.ToString(drCriterios["PREGUNTA"]);
                    objCriterio.TipoModo = Convert.ToString(drCriterios["TIPMODO"]);
                    objCriterio.IndRespuesta = Indicador.No;

                    listaCriterios.criterios.Add(objCriterio);
                    contador++;
                }

                drCriterios.Close();
                return listaCriterios;
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