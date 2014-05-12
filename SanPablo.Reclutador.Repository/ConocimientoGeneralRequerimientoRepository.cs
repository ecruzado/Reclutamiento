

namespace SanPablo.Reclutador.Repository
{

    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;
    
    
    public class ConocimientoGeneralRequerimientoRepository : Repository<ConocimientoGeneralRequerimiento>, IConocimientoGeneralRequerimientoRepository
    {
        public ConocimientoGeneralRequerimientoRepository(ISession session)
            : base(session)
        {
        }

        public List<ConocimientoGeneralRequerimiento> listarConocimientosPublicacion(int IdeSolReq)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            List<ConocimientoGeneralRequerimiento> lista = new List<ConocimientoGeneralRequerimiento>();

            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_CONO_SOLREQ_PUBLICA");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_idesolreq", OracleType.Int32).Value = IdeSolReq;
                cmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                ConocimientoGeneralRequerimiento conocimientoCargo;
                using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        conocimientoCargo = new ConocimientoGeneralRequerimiento();
                        conocimientoCargo.IdeConocimientoGeneralRequerimiento = Convert.ToInt32(lector["IDECONOGENSOLREQ"]);
                        conocimientoCargo.DescripcionConocimientoGeneral = Convert.ToString(lector["TIPOCONO"]);
                        conocimientoCargo.NombreConocimientoGeneral = Convert.ToString(lector["NOMBRE"]);
                        lista.Add(conocimientoCargo);
                    }
                }

                return lista;
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
