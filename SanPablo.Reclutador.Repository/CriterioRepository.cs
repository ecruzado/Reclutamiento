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

                while (drCriterios.Read())
                {
                    objCriterio = new Criterio();
                    
                    objCriterio.IdeCriterio = Convert.ToInt32(drCriterios["IDECRITERIO"]);
                    objCriterio.IdeSubCategoria = Convert.ToInt32(drCriterios["IDESUBCATEGORIA"]);
                    objCriterio.NombreSubCategoria = Convert.ToString(drCriterios["NOMSUBCATEGORIA"]);
                    objCriterio.Tiempo = Convert.ToInt32(drCriterios["TIEMPO"]);
                    objCriterio.Pregunta = Convert.ToString(drCriterios["PREGUNTA"]);
                    objCriterio.TipoModo = Convert.ToString(drCriterios["TIPMODO"]);
                    objCriterio.IndRespuesta = Indicador.No;

                    listaCriterios.criterios.Add(objCriterio);
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