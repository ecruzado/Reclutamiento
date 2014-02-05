﻿namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class NivelAcademicoCargoRepository : Repository<NivelAcademicoCargo>, INivelAcademicoCargoRepository
    {
        public NivelAcademicoCargoRepository(ISession session)
            : base(session)
        { 
        }
     }
}