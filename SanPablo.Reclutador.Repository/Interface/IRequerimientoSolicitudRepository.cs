using SanPablo.Reclutador.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanPablo.Reclutador.Repository.Interface
{
    public interface IRequerimientoSolicitudRepository
    {
        List<SolReqPersonal> GetBy(int ideCargo);
    }
}
