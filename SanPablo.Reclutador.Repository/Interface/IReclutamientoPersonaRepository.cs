 

namespace SanPablo.Reclutador.Repository.Interface
{
   using SanPablo.Reclutador.Entity;
    
    public interface IReclutamientoPersonaRepository : IRepository<ReclutamientoPersona>
    {

        void FinalizaContratacion(ReclutamientoPersona objRecluta);
        string validaFinSolicitud(ReclutamientoPersona objReCluta);


        /// <summary>
        /// Obtener el IdReclutamientoPersona
        /// </summary>
        /// <param name="idePostulante"></param>
        /// <param name="ideSede"></param>
        /// <param name="estadoPostulante"></param>
        /// <returns></returns>
        int getIdeReclutaPersona(int idePostulante, int ideSede);
    }

    
}
