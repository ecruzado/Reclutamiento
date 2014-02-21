namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System;
    using System.Linq.Expressions;

    public interface IHorarioCargoRepository : IRepository<HorarioCargo>
    {
        HorarioCargo getMaxPuntValue(Expression<Func<HorarioCargo, bool>> condition);
    }
}