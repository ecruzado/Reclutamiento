﻿namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class ListaSolicitudNuevoCargoVistaRepository : Repository<ListaSolicitudNuevoCargo>, IListaSolicitudNuevoCargoVistaRepository
    {
        public ListaSolicitudNuevoCargoVistaRepository(ISession session)
            : base(session)
        { 
        }
    }
}
