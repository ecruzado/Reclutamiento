namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public interface ICriterioRepository : IRepository<Criterio>
    {
        IList<Criterio> ObtenerListaMarciana(string codigo);

        ListaCriterios ObtenerCriteriosPorCategoria(int IdeCategoria);
    }
}