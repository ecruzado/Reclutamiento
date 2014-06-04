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

        //public List<string> obtenerDatosArea(SolicitudNuevoCargo solNuevo)
        //{
        //    OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
        //    try
        //    {
        //        lcon.Open();
        //        OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_CONSULTAR_DATOS_AREA");
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Connection = lcon;

        //        cmd.Parameters.Add("p_ideArea", OracleType.Int32).Value = solNuevo.IdeArea;
        //        cmd.Parameters.Add("p_ideDepartamento", OracleType.Int32).Value = solNuevo.IdeDepartamento;
        //        cmd.Parameters.Add("p_ideDependencia", OracleType.Int32).Value = solNuevo.IdeDependencia;
        //        cmd.Parameters.Add("p_ideSede", OracleType.Int32).Value = solNuevo.IdeSede;
        //        cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;
        //        cmd.ExecuteNonQuery();

        //        List<string> datos;
        //        using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
        //        {
        //            datos = new List<string>(6);

        //            if (lector.Read())
        //            {
        //                datos.Add(Convert.ToString(lector["IDEAREA"]));
        //                datos.Add(Convert.ToString(lector["NOMAREA"]));
        //                datos.Add(Convert.ToString(lector["IDEDEPARTAMENTO"]));
        //                datos.Add(Convert.ToString(lector["NOMDEPARTAMENTO"]));
        //                datos.Add(Convert.ToString(lector["IDEDEPENDENCIA"]));
        //                datos.Add(Convert.ToString(lector["NOMDEPENDENCIA"]));


        //            }
        //        }
        //        return datos;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        lcon.Close();
        //    }
        //}
        public bool verificarCodCodigo(string codigoCargo, int ideSede)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.FN_VERIFICAR_CODCARGO");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_codCargo", OracleType.VarChar).Value = codigoCargo;
                cmd.Parameters.Add("p_idSede", OracleType.Int32).Value = ideSede;
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

        public SolicitudNuevoCargo insertarSolicitudNuevo(SolicitudNuevoCargo solicitudNuevo, LogSolicitudNuevoCargo logSolicitudNuevo, string indArea)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            
            SolicitudNuevoCargo objSol = new SolicitudNuevoCargo();

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
                cmd.Parameters.Add("p_ideUsuarioResp", OracleType.Number).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("p_ideSol", OracleType.Number).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                
                objSol.IdeUsuarioResponsable= Convert.ToInt32(cmd.Parameters["p_ideUsuarioResp"].Value);
                objSol.IdeSolicitudNuevoCargo = Convert.ToInt32(cmd.Parameters["p_ideSol"].Value);

                return objSol;
                    
                    

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
        /// identifica al usuario y responsable de la publicació
        /// </summary>
        /// <param name="ideCargo"></param>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        public LogSolicitudNuevoCargo responsablePublicacion(int ideSolicitudNuevo, int ideSede)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            LogSolicitudNuevoCargo logSolicitud = new LogSolicitudNuevoCargo();
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_RESPONSABLE_PUBLICACION");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_idSolicitudNuevo", OracleType.Int32).Value = ideSolicitudNuevo;
                cmd.Parameters.Add("p_idSede", OracleType.Int32).Value = ideSede;
                cmd.Parameters.Add("p_idUsuarioResp", OracleType.Int32).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("p_idRolResp", OracleType.Int32).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                logSolicitud.UsuarioResponsable = Convert.ToInt32(cmd.Parameters["p_idUsuarioResp"].Value);
                logSolicitud.RolResponsable = Convert.ToInt32(cmd.Parameters["p_idRolResp"].Value);

                return logSolicitud;

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
        /// obtiene las solicitudes resultado de la busqueda inicial
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<SolicitudNuevoCargo> GetListaSolicitudNuevo(SolicitudNuevoCargo solicitud)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                string cFechaIncial = solicitud.FechaBusquedaInicio == null ? "" : String.Format("{0:dd/MM/yyyy}", solicitud.FechaBusquedaInicio);
                string cFechaFinal = solicitud.FechaBusquedaFin == null ? "" : String.Format("{0:dd/MM/yyyy}", solicitud.FechaBusquedaFin);

                IDataReader drSolicitudNuevo;
                SolicitudNuevoCargo solicitudNuevo;
                List<SolicitudNuevoCargo> listaSolicitudesNuevo;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.FN_GET_LISTACARGO");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_nIdSolicitud", OracleType.Int32).Value = solicitud.IdeSolicitudNuevoCargo;
                lspcmd.Parameters.Add("p_nIdDependencia", OracleType.Int32).Value = solicitud.IdeDependencia;
                lspcmd.Parameters.Add("p_nIdDepartamento", OracleType.Int32).Value = solicitud.IdeDepartamento;
                lspcmd.Parameters.Add("p_nIdArea", OracleType.Int32).Value = solicitud.IdeArea;
                lspcmd.Parameters.Add("p_cTipEtapa", OracleType.VarChar).Value = solicitud.TipoEtapa;
                lspcmd.Parameters.Add("p_cTipResp", OracleType.Int32).Value = solicitud.TipoResponsable;
                lspcmd.Parameters.Add("p_cEstado", OracleType.VarChar).Value = solicitud.TipoEstado;
                lspcmd.Parameters.Add("p_cFecIni", OracleType.VarChar).Value = cFechaIncial;
                lspcmd.Parameters.Add("p_cFeFin", OracleType.VarChar).Value = cFechaFinal;
                lspcmd.Parameters.Add("p_cRolResp", OracleType.Number).Value = solicitud.RolResponsableActual;
                lspcmd.Parameters.Add("p_cUsrResponsable", OracleType.Number).Value = solicitud.IdeUsuarioResponsable;
                lspcmd.Parameters.Add("p_nIdSede", OracleType.Number).Value = solicitud.IdeSede;

                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                drSolicitudNuevo = (OracleDataReader)lspcmd.ExecuteReader();
                solicitudNuevo = null;
                listaSolicitudesNuevo = new List<SolicitudNuevoCargo>();



                while (drSolicitudNuevo.Read())
                {
                    solicitudNuevo = new SolicitudNuevoCargo();
                    solicitudNuevo.IdeSolicitudNuevoCargo = Convert.ToInt32(drSolicitudNuevo["IDESOLNUEVOCARGO"]);
                    solicitudNuevo.CodigoCargo = Convert.ToString(drSolicitudNuevo["CODCARGO"]);
                    solicitudNuevo.IdeCargo = Convert.ToInt32(drSolicitudNuevo["IDECARGO"]);
                    solicitudNuevo.NombreCargo = Convert.ToString(drSolicitudNuevo["NOMCARGO"]);
                    solicitudNuevo.IdeDependencia = Convert.ToInt32(drSolicitudNuevo["IDEDEPENDENCIA"]);
                    solicitudNuevo.DependenciaDescripcion = Convert.ToString(drSolicitudNuevo["NOMDEPENDENCIA"]);
                    solicitudNuevo.IdeDepartamento = Convert.ToInt32(drSolicitudNuevo["IDEDEPARTAMENTO"]);
                    solicitudNuevo.DepartamentoDescripcion = Convert.ToString(drSolicitudNuevo["NOMDEPARTAMENTO"]);
                    solicitudNuevo.IdeArea = Convert.ToInt32(drSolicitudNuevo["IDEAREA"]);
                    solicitudNuevo.AreaDescripcion = Convert.ToString(drSolicitudNuevo["NOMAREA"]);

                    solicitudNuevo.NumeroPosiciones = Convert.ToInt32(drSolicitudNuevo["NUMPOSICIONES"]);
                    solicitudNuevo.Postulantes = Convert.ToInt32(drSolicitudNuevo["POSTULANTES"]);
                    solicitudNuevo.PreSeleccionados = Convert.ToInt32(drSolicitudNuevo["PRESELECCIONADOS"]);
                    solicitudNuevo.Evaluados = Convert.ToInt32(drSolicitudNuevo["EVALUADOS"]);
                    solicitudNuevo.Seleccionados = Convert.ToInt32(drSolicitudNuevo["SELECCIONADOS"]);
                    solicitudNuevo.Contratados = Convert.ToInt32(drSolicitudNuevo["CONTRATADOS"]);

                    solicitudNuevo.IdRolSuceso = Convert.ToInt32(drSolicitudNuevo["IDROL"]);
                    solicitudNuevo.RolSuceso = Convert.ToString(drSolicitudNuevo["DSCROL"]);

                    solicitudNuevo.TipoEstado = Convert.ToString(drSolicitudNuevo["ESTADO"]);
                    solicitudNuevo.TipoEtapa = Convert.ToString(drSolicitudNuevo["TETAPA"]);

                    var fecPublicacion = Convert.ToString(drSolicitudNuevo["FECPUBLICACION"]);
                    if (fecPublicacion.Length > 0)
                    {
                        solicitudNuevo.FechaPublicacion = Convert.ToDateTime(drSolicitudNuevo["FECPUBLICACION"]);
                    }
                    var fecCreacion = Convert.ToString(drSolicitudNuevo["FECCREACION"]);
                    if (fecCreacion.Length > 0)
                    {
                        solicitudNuevo.FechaCreacion = Convert.ToDateTime(drSolicitudNuevo["FECCREACION"]);
                    }

                    var fecExpiracion = Convert.ToString(drSolicitudNuevo["FECEXPIRACION"]);
                    if (fecExpiracion.Length > 0)
                    {
                        solicitudNuevo.FechaExpiracion = Convert.ToDateTime(drSolicitudNuevo["FECEXPIRACION"]);
                    }


                    solicitudNuevo.NombreResponsable = Convert.ToString(drSolicitudNuevo["NOMRESPONSABLE"]);
                    solicitudNuevo.IndicadoPublicado = Convert.ToString(drSolicitudNuevo["PUBLICADO"]);
                    solicitudNuevo.IdeUsuarioResponsable = Convert.ToInt32(drSolicitudNuevo["USRESPONSABLE"]);

                    listaSolicitudesNuevo.Add(solicitudNuevo);
                }
                drSolicitudNuevo.Close();
                return listaSolicitudesNuevo;
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

        public List<SolicitudNuevoCargo> ListarCargos(int idSede,int idRolResponsable, int idUsuarioResponsable )
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader drSolicitudNuevo;
                SolicitudNuevoCargo solicitudNuevo;
                List<SolicitudNuevoCargo> listaSolicitudesNuevo;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_CARGOS_SOLICITUD");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_idSede", OracleType.Int32).Value = idSede;
                lspcmd.Parameters.Add("p_idUsrResp", OracleType.Int32).Value = idUsuarioResponsable;
                lspcmd.Parameters.Add("p_idRolResp", OracleType.Int32).Value = idRolResponsable;

                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                drSolicitudNuevo = (OracleDataReader)lspcmd.ExecuteReader();
                solicitudNuevo = null;
                listaSolicitudesNuevo = new List<SolicitudNuevoCargo>();



                while (drSolicitudNuevo.Read())
                {
                    solicitudNuevo = new SolicitudNuevoCargo();
                    solicitudNuevo.IdeSolicitudNuevoCargo = Convert.ToInt32(drSolicitudNuevo["IDESOLNUEVOCARGO"]);
                    solicitudNuevo.NombreCargo = Convert.ToString(drSolicitudNuevo["NOMBRE"]);
                    listaSolicitudesNuevo.Add(solicitudNuevo);
                }
                drSolicitudNuevo.Close();
                return listaSolicitudesNuevo;
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
