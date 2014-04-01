 

namespace SanPablo.Reclutador.Repository.Interface
{
   using SanPablo.Reclutador.Entity;
    
    public interface IReclutamientoPersonaAlternativaRepository : IRepository<ReclutamientoPersonaAlternativa>
    {
        /// <summary>
        /// Guardar en las tablas correspondientes la respuesta del postulante
        /// </summary>
        /// <param name="ideReclutaPersona"></param>
        /// <param name="ideCriterioSubCategoria"></param>
        /// <param name="ideAlternativa"></param>
        /// <param name="usuarioCreacion"></param>
        /// <returns></returns>
        bool guardarRespuesta(int ideReclutaPersona, int ideCriterioSubCategoria, int ideAlternativa, string usuarioCreacion);

    }

    
}
