using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanPablo.Reclutador.Repository
{
    public class RequerimientoSolicitudRepository : IRequerimientoSolicitudRepository
    {
        public List<SolReqPersonal> GetBy(int ideCargo)
        {
            return new List<SolReqPersonal>();
        }
    }
}
