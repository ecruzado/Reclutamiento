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
        public bool verificarCodCodigo(string  codigoCargo)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.FN_VERIFICAR_CODCARGO");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_codCargo", OracleType.VarChar).Value = codigoCargo;
                cmd.Parameters.Add("c_retVal", OracleType.Number).Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();

                int indicador = Convert.ToInt32(cmd.Parameters[cmd.Parameters.IndexOf("c_retVal")].Value);
                if (indicador == 1)
                { return true; }
                else
                {
                    return false;
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
        public Int32 insertarSolicitudNuevo(SolicitudNuevoCargo solicitudNuevo, LogSolicitudNuevoCargo logSolicitudNuevo, string indArea)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_INSERTAR_NUEVO");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideSede", OracleType.Int32).Value = solicitudNuevo.IdeSede;
                cmd.Parameters.Add("p_ideArea", OracleType.Int32).Value = solicitudNuevo.IdeArea;
                cmd.Parameters.Add("p_codCargo", OracleType.VarChar).Value = solicitudNuevo.CodigoCargo;
                cmd.Parameters.Add("p_nomCargo", OracleType.VarChar).Value = solicitudNuevo.NombreCargo;
                cmd.Parameters.Add("p_descCargo", OracleType.VarChar).Value = solicitudNuevo.DescripcionCargo;
                cmd.Parameters.Add("p_numPosiciones", OracleType.Int32).Value = solicitudNuevo.NumeroPosiciones;
                cmd.Parameters.Add("p_tipRangoSalario", OracleType.VarChar).Value = solicitudNuevo.TipoRangoSalarial;
                cmd.Parameters.Add("p_ideDependencia", OracleType.Int32).Value = solicitudNuevo.IdeDependencia;
                cmd.Parameters.Add("p_ideDepartamento", OracleType.Int32).Value = solicitudNuevo.IdeDepartamento;
                cmd.Parameters.Add("p_estudios", OracleType.VarChar).Value = solicitudNuevo.DescripcionEstudios;
                cmd.Parameters.Add("p_funciones", OracleType.VarChar).Value = solicitudNuevo.DescripcionFunciones;
                cmd.Parameters.Add("p_competencias", OracleType.VarChar).Value = solicitudNuevo.DescripcionCompetencias;
                cmd.Parameters.Add("p_observacion", OracleType.VarChar).Value = solicitudNuevo.DescripcionObservaciones;
                cmd.Parameters.Add("p_ideUsuarioSuceso", OracleType.Int32).Value = logSolicitudNuevo.UsuarioSuceso;
                cmd.Parameters.Add("p_ideRolSuceso", OracleType.Int32).Value = logSolicitudNuevo.RolSuceso;
                cmd.Parameters.Add("p_ideRolResponsable", OracleType.Int32).Value = logSolicitudNuevo.RolResponsable;
                cmd.Parameters.Add("p_indArea", OracleType.VarChar).Value = indArea;
                cmd.Parameters.Add("p_usuarioCreacion", OracleType.VarChar).Value = solicitudNuevo.UsuarioCreacion;
                cmd.Parameters.Add("p_etapa", OracleType.VarChar).Value = logSolicitudNuevo.TipoEtapa;
                cmd.Parameters.Add("p_ideUsuarioResp", OracleType.Int32).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                return Convert.ToInt32(cmd.Parameters["p_ideUsuarioResp"].Value);

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
