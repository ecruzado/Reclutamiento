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

    public class LogSolicitudRequerimientoRepository : Repository<LogSolReqPersonal>, ILogSolicitudRequerimientoRepository
    {
        public LogSolicitudRequerimientoRepository(ISession session)
            : base(session)
        {
        }
        //public LogSolicitudNuevoCargo getMostRecentValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition)
        //{
        //    var maxResultDate = QueryOver.Of<LogSolicitudNuevoCargo>()
        //        .Where(condition)
        //        .Select(Projections.Max<LogSolicitudNuevoCargo>(x => x.FechaSuceso));

        //    return _session.QueryOver<LogSolicitudNuevoCargo>()
        //                   .Where(condition)
        //                   .WithSubquery.WhereProperty(x => x.FechaSuceso).Eq(maxResultDate)
        //                    //or to get the "last" 2 rows...
        //                    //.OrderBy(x => x.ResultDate).Desc
        //                    //.Take(2)
        //                    //.List();
        //                    .SingleOrDefault();
        //}

        //public IList<LogSolicitudNuevoCargo> getTwoMostRecentValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition)
        //{
        //    var maxResultDate = QueryOver.Of<LogSolicitudNuevoCargo>()
        //        .Where(condition)
        //        .Select(Projections.Max<LogSolicitudNuevoCargo>(x => x.FechaSuceso));

        //    return _session.QueryOver<LogSolicitudNuevoCargo>()
        //                   .Where(condition)
        //                   .WithSubquery.WhereProperty(x => x.FechaSuceso).Eq(maxResultDate)
        //                    //or to get the "last" 2 rows...
        //                   //.OrderBy(x => x.FechaSuceso).Desc
        //                   .Take(2)
        //                   .List();
        //                   // .SingleOrDefault();
        //}

        public LogSolReqPersonal getFirthValue(Expression<Func<LogSolReqPersonal, bool>> condition)
        {
            var minResultDate = QueryOver.Of<LogSolReqPersonal>()
                .Where(condition)
                .Select(Projections.Min<LogSolReqPersonal>(x => x.FechaCreacion));

            return _session.QueryOver<LogSolReqPersonal>()
                           .Where(condition)
                           .WithSubquery.WhereProperty(x => x.FechaCreacion).Eq(minResultDate)
                           .SingleOrDefault();
        }

        public int solicitarAprobacion(LogSolReqPersonal logSolicitud,int ideSolicitudRequerimiento, int ideSede, int ideArea, string indArea)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_INSERTAR_APROB_AMP");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideSolRequerimiento", OracleType.Int32).Value = ideSolicitudRequerimiento;
                cmd.Parameters.Add("p_ideRolResponsable", OracleType.Int32).Value = logSolicitud.RolResponsable;
                cmd.Parameters.Add("p_ideSede", OracleType.Int32).Value = ideSede ;
                cmd.Parameters.Add("p_ideArea", OracleType.Int32).Value = ideArea;
                cmd.Parameters.Add("p_indicArea", OracleType.VarChar).Value = indArea;
                cmd.Parameters.Add("p_tipoEtapa", OracleType.VarChar).Value = logSolicitud.TipEtapa;
                cmd.Parameters.Add("p_usuarioSuceso", OracleType.Int32).Value = logSolicitud.UsrSuceso;
                cmd.Parameters.Add("p_ideRolSuceso", OracleType.Int32).Value = logSolicitud.RolSuceso;
                cmd.Parameters.Add("p_observacion", OracleType.VarChar).Value = logSolicitud.Observacion;
                cmd.Parameters.Add("c_ideUsuarioResp", OracleType.Number).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                int ideResponsable = Convert.ToInt32(cmd.Parameters[cmd.Parameters.IndexOf("c_ideUsuarioResp")].Value);
                if (ideResponsable == null)
                { return 0; }
                else
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

        public LogSolReqPersonal estadoSolicitud(int ideSolicitud)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_REQUERIMIENTOS.SP_OBTENER_ETAPA_SOLREQ");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideSolRequerimiento", OracleType.Int32).Value = ideSolicitud;
                cmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                LogSolReqPersonal estadoSolicitud = new LogSolReqPersonal();
                using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        estadoSolicitud.TipEtapa = Convert.ToString(lector["TIPETAPA"]);
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
