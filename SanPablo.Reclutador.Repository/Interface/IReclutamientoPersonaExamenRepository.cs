 

namespace SanPablo.Reclutador.Repository.Interface
{
   using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;
    
    public interface IReclutamientoPersonaExamenRepository : IRepository<ReclutamientoPersonaExamen>
    {
        List<ReclutamientoPersonaExamen> obtenerEvaluacionesPostulante(int idePostulante, int idReclutaPersona);
    }

    
}
