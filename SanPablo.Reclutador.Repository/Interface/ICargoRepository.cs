namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
using System;
using System.Collections.Generic;
using System.Data;

    public interface ICargoRepository : IRepository<Cargo>
    {
        DatosCargo obtenerDatosCargo(int IdeSolicitud, string IdeUSuarioCreacion);

        /// <summary>
        /// CREA UNA COPIA DE CARGO PARA REALIZAR MODIFICACIONES
        /// </summary>
        /// <param name="ideCargo"></param>
        /// <param name="IdeUSuarioCreacion"></param>
        int mantenimientoCargo(int ideCargo, string IdeUSuarioCreacion);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cargo"></param>
        /// <param name="estado"></param>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        List<ListaSolicitudNuevoCargo> listaCargos(Cargo cargo, string estado, int ideSede);

        /// <summary>
        /// Consultar la etapa del cargo
        /// </summary>
        /// <param name="ideCargo"></param>
        /// <returns></returns>
        string consultaEditarCargo(int ideCargo);
    }
}