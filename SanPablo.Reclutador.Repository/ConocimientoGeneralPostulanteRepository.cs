﻿namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;

    public class ConocimientoGeneralPostulanteRepository : Repository<ConocimientoGeneralPostulante>, IConocimientoGeneralPostulanteRepository
    {
        public ConocimientoGeneralPostulanteRepository(ISession session)
            : base(session)
        {
        }

        
    }
}
