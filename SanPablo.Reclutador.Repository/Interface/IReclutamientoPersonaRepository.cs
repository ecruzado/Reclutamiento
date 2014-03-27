 

namespace SanPablo.Reclutador.Repository.Interface
{
   using SanPablo.Reclutador.Entity;
    
    public interface IReclutamientoPersonaRepository : IRepository<ReclutamientoPersona>
    {

        void FinalizaContratacion(ReclutamientoPersona objRecluta);

    }

    
}
