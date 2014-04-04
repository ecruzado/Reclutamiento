namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System;
    using System.Collections.Generic;

    public interface ISolicitudNuevoCargoRepository : IRepository<SolicitudNuevoCargo>
    {
        /// <summary>
        /// obtener los datos de Area
        /// </summary>
        /// <param name="ideArea"></param>
        /// <returns></returns>
        List<string> obtenerDatosArea(int ideArea);

        /// <summary>
        /// Verificar si el codigo de cargo ya existe en la sede
        /// </summary>
        /// <param name="codigoCargo"></param>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        bool verificarCodCodigo(string codigoCargo, int ideSede);

        /// <summary>
        /// insertar una nueva solicitud
        /// </summary>
        /// <param name="solicitudNuevo"></param>
        /// <param name="logSolicitudNuevo"></param>
        /// <param name="indArea"></param>
        /// <returns></returns>
        Int32 insertarSolicitudNuevo(SolicitudNuevoCargo solicitudNuevo, LogSolicitudNuevoCargo logSolicitudNuevo, string indArea);


        LogSolicitudNuevoCargo responsablePublicacion(int ideCargo, int ideSede);

        List<SolicitudNuevoCargo> GetListaSolicitudNuevo(SolicitudNuevoCargo solicitud);

        List<SolicitudNuevoCargo> ListarCargos(int idSede, int idRolResponsable, int idUsuarioResponsable);
    }

}