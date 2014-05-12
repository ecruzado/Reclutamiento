

namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;
    
    public interface IConocimientoGeneralRequerimientoRepository : IRepository<ConocimientoGeneralRequerimiento>
    {

        /// <summary>
        /// lista los conocimientos generales
        /// </summary>
        /// <param name="IdeSolReq"></param>
        /// <returns></returns>
        List<ConocimientoGeneralRequerimiento> listarConocimientosPublicacion(int IdeSolReq);
    }

}
