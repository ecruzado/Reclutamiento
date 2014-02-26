namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;

    public interface ILogSolicitudNuevoCargoRepository : IRepository<LogSolicitudNuevoCargo>
    {
        LogSolicitudNuevoCargo getMostRecentValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition);

        IList<LogSolicitudNuevoCargo> getTwoMostRecentValue(Expression<Func<LogSolicitudNuevoCargo, bool>> condition);

        int solicitarAprobacion(SolicitudNuevoCargo solicitud, int ideUsuario,
                                       int ideRol, string observacion, string suceso, string etapa);

        LogSolicitudNuevoCargo estadoSolicitud(int ideSolicitudNuevo);
    }
}