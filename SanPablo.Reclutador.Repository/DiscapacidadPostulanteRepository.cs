﻿namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class DiscapacidadPostulanteRepository : Repository<DiscapacidadPostulante>, IDiscapacidadPostulanteRepository
    {
        public DiscapacidadPostulanteRepository(ISession session)
            : base(session)
        {
        }
    }
}
