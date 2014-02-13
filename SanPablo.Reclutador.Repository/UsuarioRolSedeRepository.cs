﻿

namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;


    public class UsuarioRolSedeRepository : Repository<UsuarioRolSede>, IUsuarioRolSedeRepository
    {
        public UsuarioRolSedeRepository(ISession session)
            : base(session)
        {
        }
    }

}
