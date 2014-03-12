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


        public LogSolicitudNuevoCargo getFirthValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition)
        {
            var minResultDate = QueryOver.Of<LogSolicitudNuevoCargo>()
                .Where(condition)
                .Select(Projections.Min<LogSolicitudNuevoCargo>(x => x.FechaSuceso));

            return _session.QueryOver<LogSolicitudNuevoCargo>()
                           .Where(condition)
                           .WithSubquery.WhereProperty(x => x.FechaSuceso).Eq(minResultDate)
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

        public int solicitarAprobacion(LogSolicitudNuevoCargo logSolicitud,int idSede, int idArea, string indArea)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_INSERTAR_LOG");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideSolicitudNuevo", OracleType.Int32).Value = logSolicitud.IdeSolicitudNuevoCargo;
                cmd.Parameters.Add("p_ideSede", OracleType.Int32).Value = idSede;
                cmd.Parameters.Add("p_ideArea", OracleType.Int32).Value = idArea;
                cmd.Parameters.Add("p_ideUsuarioSuceso", OracleType.Int32).Value = logSolicitud.UsuarioSuceso;
                cmd.Parameters.Add("p_ideRolSuceso", OracleType.Int32).Value = logSolicitud.RolSuceso;
                cmd.Parameters.Add("p_ideRolResponsable", OracleType.Int32).Value = logSolicitud.RolResponsable;
                cmd.Parameters.Add("p_indArea", OracleType.VarChar).Value = indArea;
                cmd.Parameters.Add("p_etapa", OracleType.VarChar).Value = logSolicitud.TipoEtapa;
                cmd.Parameters.Add("p_ideUsuarioResp", OracleType.Number).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                int ideResponsable = Convert.ToInt32(cmd.Parameters[cmd.Parameters.IndexOf("p_ideUsuarioResp")].Value);
                if (ideResponsable == null)
                { return -1; } else
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

        public LogSolicitudNuevoCargo estadoSolicitud(int ideSolicitudNuevo)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_OBTENER_ETAPA_SOLICITUD");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideSolCargo", OracleType.Int32).Value = ideSolicitudNuevo;
                cmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                LogSolicitudNuevoCargo estadoSolicitud = new LogSolicitudNuevoCargo();
                using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        estadoSolicitud.TipoEtapa = Convert.ToString(lector["TIPETAPA"]);
                        estadoSolicitud.RolResponsable = Convert.ToInt32(lector["ROLRESPONSABLE"]);
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
