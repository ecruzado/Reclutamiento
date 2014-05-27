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

        public DatosCargo obtenerDatosCargo(int IdeSolicitud, string IdeUSuarioCreacion, int IdeSede)
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
                cmd.Parameters.Add("p_ideSede", OracleType.Int32).Value = IdeSede;
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

        /// <summary>
        /// CREA UNA COPIA DE CARGO PARA REALIZAR MODIFICACIONES
        /// </summary>
        /// <param name="ideCargo"></param>
        /// <param name="IdeUSuarioCreacion"></param>
        public int  mantenimientoCargo(int ideCargo, string IdeUSuarioCreacion)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_MANTENIMIENTO_CARGO");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideCargo", OracleType.Int32).Value = ideCargo;
                cmd.Parameters.Add("p_usrCreacion", OracleType.VarChar).Value = IdeUSuarioCreacion;
                cmd.Parameters.Add("p_idCargoCopia", OracleType.Int32).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int ideCargoNuevo = Convert.ToInt32(cmd.Parameters["p_idCargoCopia"].Value);

                return ideCargoNuevo;
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
        /// lista de cargos para lista desplegable de ampliacion
        /// </summary>
        /// <param name="IdCargo"></param>
        /// <returns></returns>
        public List<Cargo> listaCargosCompletos(int IdeSede)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                IDataReader ldrCargo;
                Cargo lobCargo;
                List<Cargo> llstCargo;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_REQUERIMIENTOS.SP_LISTACARGOS");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_idSede", OracleType.Int32).Value = IdeSede;
                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;
                ldrCargo = (OracleDataReader)lspcmd.ExecuteReader();
                lobCargo = null;
                llstCargo = new List<Cargo>();


                while (ldrCargo.Read())
                {
                    lobCargo = new Cargo();
                    lobCargo.IdeCargo = Convert.ToInt32(ldrCargo["IDECARGO"]);
                    lobCargo.NombreCargo = Convert.ToString(ldrCargo["NOMCARGO"]);
                    llstCargo.Add(lobCargo);
                }
                ldrCargo.Close();
                return llstCargo;
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
        /// Listar los cargos para mantenimiento
        /// </summary>
        /// <param name="IdeSolicitud"></param>
        /// <param name="IdeUSuarioCreacion"></param>
        /// <returns></returns>
        public List<ListaSolicitudNuevoCargo> listaCargosMantenimiento(Cargo cargo, string estado, int ideSede)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                string cFechaIncial = cargo.FechaInicio == null ? "" : String.Format("{0:dd/MM/yyyy}", cargo.FechaInicio);
                string cFechaFinal = cargo.FechaFin == null ? "" : String.Format("{0:dd/MM/yyyy}", cargo.FechaFin);

                lcon.Open();
                IDataReader drCargos;
                ListaSolicitudNuevoCargo objetoCargo;
                List<ListaSolicitudNuevoCargo> listaCargo = new List<ListaSolicitudNuevoCargo>();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_LISTA_CARGOS_MANT");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_nCodCargo", OracleType.VarChar).Value = cargo.CodigoCargo;
                cmd.Parameters.Add("p_nIdDependencia", OracleType.Int32).Value = cargo.IdeDependencia;
                cmd.Parameters.Add("p_nIdDepartamento", OracleType.Int32).Value = cargo.IdeDepartamento;
                cmd.Parameters.Add("p_nIdArea", OracleType.Int32).Value = cargo.IdeArea;
                cmd.Parameters.Add("p_cFecIni", OracleType.VarChar).Value = cFechaIncial;
                cmd.Parameters.Add("p_cFeFin", OracleType.VarChar).Value = cFechaFinal;
                cmd.Parameters.Add("p_cEstado", OracleType.VarChar).Value = estado;
                cmd.Parameters.Add("p_cideSede", OracleType.Int32).Value = ideSede;
                cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                drCargos = (OracleDataReader)cmd.ExecuteReader();
                objetoCargo = null;

                while (drCargos.Read())
                {
                    objetoCargo = new ListaSolicitudNuevoCargo();

                    if (drCargos["ESTACTIVO"] != System.DBNull.Value)
                    {
                        objetoCargo.EstadoActivo = Convert.ToString(drCargos["ESTACTIVO"]);
                    }
                    if (drCargos["IDECARGO"] != System.DBNull.Value)
                    {
                        objetoCargo.IdeCargo = Convert.ToInt32(drCargos["IDECARGO"]);
                    }
                    if (drCargos["CODCARGO"] != System.DBNull.Value)
                    {
                        objetoCargo.CodigoCargo = Convert.ToString(drCargos["CODCARGO"]);
                    }
                    if (drCargos["NOMCARGO"] != System.DBNull.Value)
                    {
                        objetoCargo.NombreCargo = Convert.ToString(drCargos["NOMCARGO"]);
                    }
                    if (drCargos["DESCARGO"] != System.DBNull.Value)
                    {
                        objetoCargo.DescripcionCargo = Convert.ToString(drCargos["DESCARGO"]);
                    }
                    if (drCargos["IDEDEPENDENCIA"] != System.DBNull.Value)
                    {
                        objetoCargo.IdeDependencia = Convert.ToInt32(drCargos["IDEDEPENDENCIA"]);
                    }
                    if (drCargos["NOMDEPENDENCIA"] != System.DBNull.Value)
                    {
                        objetoCargo.NombreDependencia = Convert.ToString(drCargos["NOMDEPENDENCIA"]);
                    }
                    if (drCargos["IDEDEPARTAMENTO"] != System.DBNull.Value)
                    {
                        objetoCargo.IdeDepartamento = Convert.ToInt32(drCargos["IDEDEPARTAMENTO"]);
                    }
                    if (drCargos["NOMDEPARTAMENTO"] != System.DBNull.Value)
                    {
                        objetoCargo.NombreDepartamento = Convert.ToString(drCargos["NOMDEPARTAMENTO"]);
                    }
                    if (drCargos["IDEAREA"] != System.DBNull.Value)
                    {
                        objetoCargo.IdeArea = Convert.ToInt32(drCargos["IDEAREA"]);
                    }
                    if (drCargos["NOMAREA"] != System.DBNull.Value)
                    {
                        objetoCargo.NombreArea = Convert.ToString(drCargos["NOMAREA"]);
                    }
                    if (drCargos["VERSION"] != System.DBNull.Value)
                    {
                        objetoCargo.version = Convert.ToInt32(drCargos["VERSION"]);
                    }
                    listaCargo.Add(objetoCargo);
                }
                drCargos.Close();
                return listaCargo;
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

        public string consultaEditarCargo(int ideCargo)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_CONSULTA_EDITAR_CARGO");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideCargo", OracleType.Int32).Value = ideCargo;
                cmd.Parameters.Add("p_cRetVal", OracleType.VarChar,10).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                string etapaCargo = Convert.ToString(cmd.Parameters["p_cRetVal"].Value);

                return etapaCargo;
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
        /// devuleve los cargo y sus codigos and id
        /// </summary>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        public List<Cargo> listarCargosSedeCodigo(int ideSede)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                IDataReader drCargos;
                Cargo objetoCargo;
                List<Cargo> listaCargos = new List<Cargo>();

                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_CARGOS_MANT");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideSede", OracleType.Int32).Value = ideSede;
                cmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                drCargos = (OracleDataReader)cmd.ExecuteReader();
                objetoCargo = null;


                while (drCargos.Read())
                {
                    objetoCargo = new Cargo();
                    if (drCargos["CODCARGO"] != System.DBNull.Value)
                    {
                        objetoCargo.CodigoCargo = drCargos["CODCARGO"].ToString();
                    }
                    if (drCargos["IDECARGO"] != System.DBNull.Value)
                    {
                        objetoCargo.IdeCargo = Convert.ToInt32(drCargos["IDECARGO"]);
                    }
                    if (drCargos["NOMCARGO"] != System.DBNull.Value)
                    {
                        objetoCargo.NombreCargo = Convert.ToString(drCargos["NOMCARGO"]);
                    }

                    listaCargos.Add(objetoCargo);
                }
                drCargos.Close();
                return listaCargos;
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

        public List<Cargo> listarCargosSede(int ideSede)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                IDataReader drCargos;
                Cargo objetoCargo;
                List<Cargo> listaCargos = new List<Cargo>();
                
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_CARGOS_SEDE");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideSede", OracleType.Int32).Value = ideSede;
                cmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                drCargos = (OracleDataReader)cmd.ExecuteReader();
                objetoCargo = null;


                while (drCargos.Read())
                {
                    objetoCargo = new Cargo();

                    objetoCargo.IdeCargo = Convert.ToInt32(drCargos["IDECARGO"]);
                    objetoCargo.NombreCargo = Convert.ToString(drCargos["NOMCARGO"]);

                    listaCargos.Add(objetoCargo);
                }
                drCargos.Close();
                return listaCargos;
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
