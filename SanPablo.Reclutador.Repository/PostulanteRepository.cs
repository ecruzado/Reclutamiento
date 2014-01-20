﻿namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class PostulanteRepository : Repository<Persona>, IPostulanteRepository
    {
        public PostulanteRepository(ISession session)
            : base(session)
        {
        }
    }
}