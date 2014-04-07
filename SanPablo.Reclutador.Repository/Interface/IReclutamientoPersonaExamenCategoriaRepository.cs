 

namespace SanPablo.Reclutador.Repository.Interface
{
   using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;
    
    public interface IReclutamientoPersonaExamenCategoriaRepository : IRepository<ReclutamientoPersonaExamenCategoria>
    {
        /// <summary>
        /// obtener el identificador de la categoria
        /// </summary>
        /// <param name="idReclutaPersonaExamenCategoria"></param>
        /// <returns></returns>
        int obtenerIdentificadorCategoria(int idReclutaPersonaExamenCategoria);

        /// <summary>
        /// genera los examens por categorias
        /// </summary>
        /// <param name="idReclutaPersona"></param>
        /// <param name="usuarioSession"></param>
        /// <returns></returns>
        bool obtenerExamenesPorCategoria(int idReclutaPersona, string usuarioSession);

        /// <summary>
        /// obtiene la lista de los examenes por categorias
        /// </summary>
        /// <param name="ideReclutaPersona"></param>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        List<DatosExamenPorCategoria> ListarExamenesPorCategoria(int ideReclutaPersona);
    }

    
}
