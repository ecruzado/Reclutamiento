namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;

    public class SolicitudNuevoCargoRepository : Repository<SolicitudNuevoCargo>, ISolicitudNuevoCargoRepository
    {
        public SolicitudNuevoCargoRepository(ISession session)
            : base(session)
        {
        }
        
        public List<string> obtenerDatosArea(int ideArea)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_CONSULTAR_DATOS_AREA");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideArea", OracleType.Int32).Value = ideArea;
                cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                
                List<string> datos;
                using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                {
                    datos = new List<string>(6);

                    if (lector.Read())
                    {
                        datos.Add (Convert.ToString(lector["IDEAREA"]));
                        datos.Add (Convert.ToString(lector["NOMAREA"]));
                        datos.Add (Convert.ToString(lector["IDEDEPARTAMENTO"]));
                        datos.Add (Convert.ToString(lector["NOMDEPARTAMENTO"]));
                        datos.Add (Convert.ToString(lector["IDEDEPENDENCIA"]));
                        datos.Add (Convert.ToString(lector["NOMDEPENDENCIA"]));
                        

                    }
                }
                return datos;
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
