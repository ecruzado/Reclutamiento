﻿namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;

    public interface ILogSolicitudNuevoCargoRepository : IRepository<LogSolicitudNuevoCargo>
    {
        LogSolicitudNuevoCargo getMostRecentValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition);

        LogSolicitudNuevoCargo getFirthValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition);

        IList<LogSolicitudNuevoCargo> getTwoMostRecentValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition);

        int solicitarAprobacion(LogSolicitudNuevoCargo logSolicitud, int idSede, int idArea, string indArea);

        LogSolicitudNuevoCargo estadoSolicitud(int ideSolicitudNuevo);
    }
}