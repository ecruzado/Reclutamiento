namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public interface IConocimientoGeneralCargoRepository : IRepository<ConocimientoGeneralCargo>
    {
        void actualizarPuntaje(int valor, int valorEliminado, int IdeCargo, string tipoConocimiento);

        /// <summary>
        /// listar conocimientos de publicacion
        /// </summary>
        /// <param name="IdeCargo"></param>
        /// <returns></returns>
        List<ConocimientoGeneralCargo> listarConocimientosPublicacion(int IdeCargo);
        
    }
}