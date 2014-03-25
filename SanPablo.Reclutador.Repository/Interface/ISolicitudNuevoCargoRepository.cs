namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System;
    using System.Collections.Generic;

    public interface ISolicitudNuevoCargoRepository : IRepository<SolicitudNuevoCargo>
    {
        List<string> obtenerDatosArea(int ideArea);
        bool verificarCodCodigo(string codigoCargo);

        Int32 insertarSolicitudNuevo(SolicitudNuevoCargo solicitudNuevo, LogSolicitudNuevoCargo logSolicitudNuevo, string indArea);

        LogSolicitudNuevoCargo responsablePublicacion(int ideCargo, int ideSede);

        List<SolicitudNuevoCargo> GetListaSolicitudNuevo(SolicitudNuevoCargo solicitud);
    }

}