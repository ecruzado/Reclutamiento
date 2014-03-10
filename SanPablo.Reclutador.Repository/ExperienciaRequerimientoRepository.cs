﻿
namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Data;
    using System.Data.OracleClient;

    public class ExperienciaRequerimientoRepository : Repository<ExperienciaRequerimiento>, IExperienciaRequerimientoRepository
    {
        public ExperienciaRequerimientoRepository(ISession session)
            : base(session)
        {
        }

    }
}
