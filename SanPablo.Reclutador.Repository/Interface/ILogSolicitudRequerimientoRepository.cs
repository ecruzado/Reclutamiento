namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;

    public interface ILogSolicitudRequerimientoRepository : IRepository<LogSolReqPersonal>
    {
        //LogSolicitudNuevoCargo getMostRecentValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition);

        //IList<LogSolicitudNuevoCargo> getTwoMostRecentValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition);

        LogSolReqPersonal getFirthValue(Expression<Func<LogSolReqPersonal, bool>> condition);

        int solicitarAprobacion(LogSolReqPersonal logSolicitud, int ideSolicitudRequerimiento, int ideSede, int ideArea, string indArea);

        LogSolReqPersonal estadoSolicitud(int ideSolicitud);
    }
}