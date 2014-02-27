namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;

    public class CargoRepository : Repository<Cargo>, ICargoRepository
    {
        public CargoRepository(ISession session)
            : base(session)
        {
        }

        public DatosCargo obtenerDatosCargo(int IdeSolicitud, string IdeUSuarioCreacion)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_CARGO");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = IdeSolicitud;
                cmd.Parameters.Add("p_ideUsuario", OracleType.VarChar).Value = IdeUSuarioCreacion;
                cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                DatosCargo cargo;
                using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                {
                    cargo = new DatosCargo();

                    if (lector.Read())
                    {
                        cargo.CodigoCargo = Convert.ToString(lector["CODCARGO"]);
                        cargo.NombreCargo = Convert.ToString(lector["NOMCARGO"]);
                        cargo.DescripcionCargo = Convert.ToString(lector["DESCARGO"]);
                        cargo.Area = Convert.ToString(lector["NOMAREA"]);
                        cargo.Departamento = Convert.ToString(lector["NOMDEPARTAMENTO"]);
                        cargo.Dependencia = Convert.ToString(lector["NOMDEPENDENCIA"]);
                        cargo.IdeCargo = Convert.ToInt32(lector["IDECARGO"]);
                        cargo.NumeroPosiciones = Convert.ToInt32(lector["NUMPOSIC"]);
                       
                    }
                }

                return cargo;
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
