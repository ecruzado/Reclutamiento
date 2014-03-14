using SanPablo.Reclutador.Entity;
using System.Collections.Generic;


namespace SanPablo.Reclutador.Repository.Interface
{
    public interface IListaSolicitudNuevoCargoVistaRepository : IRepository<ListaSolicitudNuevoCargo>
    {
        List<SolicitudConsulta> ListaSolicitudesRequerimientos(SolReqPersonal busqueda);
    }
}
