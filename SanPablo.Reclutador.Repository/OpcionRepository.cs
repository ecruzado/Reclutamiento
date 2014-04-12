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

    using System.Data;

    public class OpcionRepository : Repository<Opcion>, IOpcionRepository
    {
        public OpcionRepository(ISession session)
            : base(session)
        { 
        }

        /// <summary>
        /// obtiene las opciones
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<Opcion> GetOpcion(Opcion obj)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader ldrOpcion;
                Opcion lobOpcion;
                List<Opcion> llstOpcion;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.FN_OBTIENE_OPCIONES");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;


                lspcmd.Parameters.Add("p_cdesopcion", OracleType.VarChar).Value = obj.DSCOPCION;
                lspcmd.Parameters.Add("p_cdescripcion", OracleType.VarChar).Value = (obj.DESCRIPCION==null?"":obj.DESCRIPCION);
                lspcmd.Parameters.Add("p_cestado", OracleType.VarChar).Value = (obj.FLGHABILITADO == null ? "" : obj.FLGHABILITADO);
                lspcmd.Parameters.Add("p_ctipmenu", OracleType.VarChar).Value =(obj.TIPMENU==null?"":obj.TIPMENU);

                lspcmd.Parameters.Add("p_crpta", OracleType.Cursor).Direction = ParameterDirection.Output;

                ldrOpcion = (OracleDataReader)lspcmd.ExecuteReader();
                lobOpcion = null;
                llstOpcion = new List<Opcion>();


                while (ldrOpcion.Read())
                {

                    lobOpcion = new Opcion();
                    lobOpcion.FLGHABILITADO = Convert.ToString(ldrOpcion["FLGHABILITADO"]);

                    if (ldrOpcion["IDOPCIONPADRE"] != DBNull.Value && ldrOpcion["IDOPCIONPADRE"] != null)
                    {
                        lobOpcion.IDOPCIONPADRE = Convert.ToInt32(ldrOpcion["IDOPCIONPADRE"]);

                    }
                    
                   
                    if (ldrOpcion["IDOPCION"] != DBNull.Value && ldrOpcion["IDOPCION"] != null)
                    {
                        lobOpcion.IDOPCION = Convert.ToInt32(ldrOpcion["IDOPCION"]);    
                    }

                    if (ldrOpcion["IDITEM"] != DBNull.Value && ldrOpcion["IDITEM"] != null)
                    {
                        lobOpcion.IDITEM = Convert.ToInt32(ldrOpcion["IDITEM"]);    
                    }
                    

                    

                    lobOpcion.DSCOPCION = (ldrOpcion["DSCOPCION"] == null ? "" : Convert.ToString(ldrOpcion["DSCOPCION"]));
                    lobOpcion.DESCRIPCION = Convert.ToString(ldrOpcion["DESCRIPCION"]);
                    lobOpcion.TIPMENU = Convert.ToString(ldrOpcion["TIPMENU"]);
                    lobOpcion.DESMENU = Convert.ToString(ldrOpcion["DESMENU"]);
                   
                    llstOpcion.Add(lobOpcion);

                }
                ldrOpcion.Close();
                return llstOpcion;
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