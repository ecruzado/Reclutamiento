namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;
    using System.Linq.Expressions;

    public class LogSolicitudNuevoCargoRepository : Repository<LogSolicitudNuevoCargo>, ILogSolicitudNuevoCargoRepository
    {
        public LogSolicitudNuevoCargoRepository(ISession session)
            : base(session)
        {
        }
        public LogSolicitudNuevoCargo getMostRecentValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition)
        {
            var maxResultDate = QueryOver.Of<LogSolicitudNuevoCargo>()
                .Where(condition)
                .Select(Projections.Max<LogSolicitudNuevoCargo>(x => x.FechaSuceso));

            return _session.QueryOver<LogSolicitudNuevoCargo>()
                           .Where(condition)
                           .WithSubquery.WhereProperty(x => x.FechaSuceso).Eq(maxResultDate)
                            //or to get the "last" 2 rows...
                            //.OrderBy(x => x.ResultDate).Desc
                            //.Take(2)
                            //.List();
                            .SingleOrDefault();
        }

        public IList<LogSolicitudNuevoCargo> getTwoMostRecentValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition)
        {
            var maxResultDate = QueryOver.Of<LogSolicitudNuevoCargo>()
                .Where(condition)
                .Select(Projections.Max<LogSolicitudNuevoCargo>(x => x.FechaSuceso));

            return _session.QueryOver<LogSolicitudNuevoCargo>()
                           .Where(condition)
                           .WithSubquery.WhereProperty(x => x.FechaSuceso).Eq(maxResultDate)
                            //or to get the "last" 2 rows...
                           //.OrderBy(x => x.FechaSuceso).Desc
                           .Take(2)
                           .List();
                           // .SingleOrDefault();
        }

        public int solicitarAprobacion(int ideSede, int ideArea, int ideSolicitudCargo, int ideUsuario, 
                                       int ideRol, string observacion, string suceso, string etapa)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.FN_APROBACION_NUEVO");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideSede", OracleType.Int32).Value = ideSede;
                cmd.Parameters.Add("p_ideArea", OracleType.Int32).Value = ideArea;
                cmd.Parameters.Add("p_ideSolCargo", OracleType.Int32).Value = ideSolicitudCargo;
                cmd.Parameters.Add("p_ideUsuario", OracleType.Int32).Value = ideUsuario;
                cmd.Parameters.Add("p_ideRol", OracleType.Int32).Value = ideRol;
                cmd.Parameters.Add("p_observacion", OracleType.VarChar).Value = observacion;
                cmd.Parameters.Add("p_suceso", OracleType.VarChar).Value = suceso;
                cmd.Parameters.Add("p_etapa", OracleType.VarChar).Value = etapa;
                cmd.Parameters.Add("c_ideUsuarioResp", OracleType.Number).Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                
                int ideResponsable = Convert.ToInt32(cmd.Parameters[cmd.Parameters.IndexOf("c_ideUsuarioResp")].Value);
                if (ideResponsable == null)
                { return 0; } else
                return ideResponsable;
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

        public List<string> estadoSolicitud(int ideSolicitudNuevo, int tipoEtapa, int tipoSuceso)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_OBTENER_ETAPA_SOLICITUD");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideSolCargo", OracleType.Int32).Value = ideSolicitudNuevo;
                cmd.Parameters.Add("p_ideGeneralE", OracleType.Int32).Value = tipoEtapa;
                cmd.Parameters.Add("p_ideGeneralS", OracleType.Int32).Value = tipoSuceso;
                cmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;
                
                List<string> estadoSolicitud = new List<string>();
                using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        estadoSolicitud.Add(Convert.ToString(lector["ETAPA"]));
                        estadoSolicitud.Add(Convert.ToString(lector["SUCESO"]));
                    }
                }

                return estadoSolicitud;
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
