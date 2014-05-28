namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;

    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ISession session)
            : base(session)
        { 
        }

        /// <summary>
        /// obtiene la lista de categorias discponobles para enlazar a un examen por Sede
        /// </summary>
        /// <param name="obj">Categoria</param>
        /// <returns></returns>
        public List<Categoria> ObtenerCategorias(Categoria obj)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader drCategorias;
                Categoria objCategoria;
                List<Categoria> listaCategorias = new List<Categoria>();
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_OBTIENE_CATEGORIAS");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_ctipcategoria", OracleType.VarChar).Value = (obj.TIPCATEGORIA==null?"":obj.TIPCATEGORIA);
                lspcmd.Parameters.Add("p_cdescrip", OracleType.VarChar).Value = (obj.DESCCATEGORIA==null?"":obj.DESCCATEGORIA);
                lspcmd.Parameters.Add("p_nidsede", OracleType.Number).Value = (obj.IdeSede==null?0:obj.IdeSede);
                lspcmd.Parameters.Add("p_cNombreCat", OracleType.VarChar).Value = (obj.NOMCATEGORIA == null ? "" : obj.NOMCATEGORIA);
                lspcmd.Parameters.Add("p_crpta", OracleType.Cursor).Direction = ParameterDirection.Output;
                drCategorias = (OracleDataReader)lspcmd.ExecuteReader();
                objCategoria = null;

                while (drCategorias.Read())
                {
                    objCategoria = new Categoria();


                    objCategoria.ESTACTIVO = Convert.ToString(drCategorias["ESTACTIVO"]);
                    objCategoria.IDECATEGORIA = Convert.ToInt32(drCategorias["IDECATEGORIA"]);
                    objCategoria.NOMCATEGORIA = Convert.ToString(drCategorias["NOMCATEGORIA"]);
                    objCategoria.DESCCATEGORIA = Convert.ToString(drCategorias["DESCCATEGORIA"]);
                    objCategoria.TIPCATEGORIA = Convert.ToString(drCategorias["TIPCATEGORIA"]);
                    objCategoria.TIPCATEGORIADES = Convert.ToString(drCategorias["DESTIPCATEGORIA"]);
                    

                    listaCategorias.Add(objCategoria);

                }

                drCategorias.Close();
                return listaCategorias;
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