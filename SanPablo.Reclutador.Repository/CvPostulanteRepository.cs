

namespace SanPablo.Reclutador.Repository
{

    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System.Collections.Generic;
   
    public class CvPostulanteRepository : Repository<CvPostulante>, ICvPostulanteRepository
    {
        public CvPostulanteRepository(ISession session)
            : base(session)
        {
        }
    }

}
